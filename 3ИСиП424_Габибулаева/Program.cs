using System;
using System.Collections.Generic;

namespace _3Исип424
{
    public enum EnemyType { Goblin, Skeleton, Mage }
    public enum PlayerChoice { Attack = 1, Defend = 2 }
    public enum ChestChoice { Equip = 1, Discard = 2 }

    public abstract class Item
    {
        public string Name { get; }
        public int Value { get; }

        protected Item(string name, int value)
        {
            Name = name;
            Value = value;
        }
    }

    public class Weapon : Item
    {
        public int Damage
        {
            get { 
                return Value; }
        }

        public Weapon(string name, int damage) : base(name, damage) { }
    }

    public class Armor : Item
    {
        public int Defense
        {
            get { return Value; }
        }

        public Armor(string name, int defense) : base(name, defense) { }
    }

    public abstract class Entity
    {
        public string Name { get; protected set; }
        public int MaxHp { get; protected set; }
        public int CurrentHp { get; protected set; }
        public abstract int AttackPower { get; }
        public abstract int DefensePower { get; }

        public bool IsAlive
        {
            get { return CurrentHp > 0; }
        }

        protected Entity(string name, int maxHp)
        {
            Name = name;
            MaxHp = maxHp;
            CurrentHp = maxHp;
        }

        public virtual void TakeDamage(int damage)
        {
            int finalDamage = damage;
            if (finalDamage < 0)
            {
                finalDamage = 0;
            }
            CurrentHp = CurrentHp - finalDamage;
            if (CurrentHp < 0)
            {
                CurrentHp = 0;
            }
        }

        public void HealFull()
        {
            CurrentHp = MaxHp;
        }
    }

    public class Player : Entity
    {
        public Weapon CurrentWeapon { get; private set; }
        public Armor CurrentArmor { get; private set; }
        public bool IsFrozen { get; set; }

        public override int AttackPower
        {
            get
            {
                if (CurrentWeapon != null)
                {
                    return CurrentWeapon.Damage;
                }
                return 5;
            }
        }

        public override int DefensePower
        {
            get
            {
                if (CurrentArmor != null)
                {
                    return CurrentArmor.Defense;
                }
                return 0;
            }
        }

        public Player(string name) : base(name, 1000)
        {
            CurrentWeapon = new Weapon("Кулак веры", 37);
            CurrentArmor = new Armor("В чем мама родила", 67);
            IsFrozen = false;
        }

        public void ChangeWeapon(Weapon newWeapon)
        {
            CurrentWeapon = newWeapon;
        }

        public void ChangeArmor(Armor newArmor)
        {
            CurrentArmor = newArmor;
        }
    }

    public class Enemy : Entity
    {
        private int _baseAttack;
        private int _baseDefense;
        public EnemyType Type { get; }

        public override int AttackPower
        {
            get { return _baseAttack; }
        }

        public override int DefensePower
        {
            get { return _baseDefense; }
        }

        public Enemy(string name, int hp, int attack, int defense, EnemyType type) : base(name, hp)
        {
            _baseAttack = attack;
            _baseDefense = defense;
            Type = type;
        }
    }

    public class GameEngine
    {
        private Player _player;
        private Random _random = new Random();
        private int _turnCounter = 0;

        private List<string> _weaponNames = new List<string> { "Пинок веры", "Харчокв лицо", "Хук левой", "Тесак", "Зачётная книжка" };
        private List<string> _armorNames = new List<string> { "Удобная одежда", "Мантия невидимка", "Крутая броня", "МегаКрутая броня", "СуперМегаКрутаяБроня" };

        public GameEngine(string playerName)
        {
            _player = new Player(playerName);
        }

        //Начало этого Ужаса  ಥ_ಥ я хочу домой и хочу покоя 💔💔

        public void Start()
        {
            Console.Clear();
            Console.WriteLine("Спуск в подземелье начался(к сожалению), " + _player.Name + ".");
            Console.WriteLine("Нажмите любую клавишу для старта...");
            Console.ReadKey();

            while (_player.IsAlive)
            {
                _turnCounter++;
                Console.Clear();
                Console.WriteLine("Комната " + _turnCounter );
                Console.ResetColor();

                Console.WriteLine("Вы: HP " + _player.CurrentHp + "/" + _player.MaxHp + " | " + _player.CurrentWeapon.Name + " (" + _player.AttackPower + " ATK) | " + _player.CurrentArmor.Name + " (" + _player.DefensePower + " DEF)\n");

                if (_turnCounter % 10 == 0)
                {
                    FightBoss(GenerateBoss());
                }
                else
                {
                    if (_random.Next(0, 2) == 0)
                    {
                        ChestEvent();
                    }
                    else
                    {
                        FightBoss(GenerateRandomEnemy());
                    }
                }

                if (_player.IsAlive)
                {
                    Console.WriteLine("\nТопаем дальше...");
                    Console.ResetColor();
                    Console.ReadKey();
                }
            }

            Console.Clear();
            Console.WriteLine("ВЫ ИСПУСТИЛИ ДУХ. ГГ.");
            Console.WriteLine("Пройдено комнат: " + (_turnCounter - 1));
            Console.ResetColor();
        }

        private Enemy GenerateRandomEnemy()
        {
            int typeIndex = _random.Next(0, 3);
            EnemyType type = (EnemyType)typeIndex;
            switch (type)
            {
                case EnemyType.Goblin:
                    return new Enemy("Дикий огурец", _random.Next(30, 51), _random.Next(12, 19), _random.Next(4, 9), type);
                case EnemyType.Skeleton:
                    return new Enemy("Скелетон Костянчик", _random.Next(25, 46), _random.Next(15, 23), _random.Next(2, 6), type);
                case EnemyType.Mage:
                    return new Enemy("Чёрный маг( не в плане магии)", _random.Next(20, 41), _random.Next(18, 26), _random.Next(1, 5), type);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private Enemy GenerateBoss()
        {
            int bossIndex = _random.Next(0, 4);
            switch (bossIndex)
            {
                case 0: return new Enemy("ВВГ (Гоблин-переросток)", 90, 22, 7, EnemyType.Goblin);
                case 1: return new Enemy("Ковалеский (Лорд скелетов)", 87, 23, 4, EnemyType.Skeleton);
                case 2: return new Enemy("Архимаг C++", 54, 35, 2, EnemyType.Mage);
                default: return new Enemy("Пестов С--", 45, 32, 1, EnemyType.Skeleton);
            }
        }

        private void ChestEvent()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("На полу стоит старый сундук.");
            Console.ResetColor();

            int lootType = _random.Next(0, 3);
            if (lootType == 0)
            {
                Console.WriteLine("Внутри зелье лжи. Здоровье восстановлено.");
                Console.ResetColor();
                _player.HealFull();
            }
            else if (lootType == 1)
            {
                Weapon newWeapon = new Weapon(_weaponNames[_random.Next(_weaponNames.Count)], _random.Next(1, 999));
                Console.WriteLine("Найдено: " + newWeapon.Name + " (Урон: " + newWeapon.Damage + ")");
                Console.WriteLine("В руках: " + _player.CurrentWeapon.Name + " (Урон: " + _player.AttackPower + ")");

                if (GetChestChoice("1. Взять\n2. АнтиВзять") == ChestChoice.Equip)
                {
                    _player.ChangeWeapon(newWeapon);
                    Console.WriteLine("Вы сменили оружие.");
                }
            }
            else
            {
                Armor newArmor = new Armor(_armorNames[_random.Next(_armorNames.Count)], _random.Next(1, 999));
                Console.WriteLine("Найдено: " + newArmor.Name + " (Защита: " + newArmor.Defense + ")");
                Console.WriteLine("Надето: " + _player.CurrentArmor.Name + " (Защита: " + _player.DefensePower + ")");

                if (GetChestChoice("1. Надевать\n2. АнтиНадевать") == ChestChoice.Equip)
                {
                    _player.ChangeArmor(newArmor);
                    Console.WriteLine("Вы переоделись.");
                }
            }
        }

        private void FightBoss(Enemy enemy)
        {
            Console.WriteLine("Перед вами " + enemy.Name + " [HP: " + enemy.CurrentHp + " | ATK: " + enemy.AttackPower + " | DEF: " + enemy.DefensePower + "]");
            Console.ResetColor();

            while (_player.IsAlive && enemy.IsAlive)
            {
                bool playerEvaded = false;
                bool isDefending = false;

                if (_player.IsFrozen)
                {
                    Console.WriteLine("Вас сморозили, скип хода. Как жаль.");
                    Console.ResetColor();
                    _player.IsFrozen = false;
                }
                else
                {
                    Console.WriteLine("\nВаше HP: " + _player.CurrentHp + " | HP врага: " + enemy.CurrentHp);
                    if (GetPlayerChoice("1. Рубить\n2. Дефаться") == PlayerChoice.Attack)
                    {
                        int dmgToEnemy = _player.AttackPower - enemy.DefensePower;
                        if (dmgToEnemy < 1)
                        {
                            dmgToEnemy = 1;
                        }
                        enemy.TakeDamage(dmgToEnemy);
                        Console.WriteLine("Вы нанесли " + dmgToEnemy + " урона. Маладес");
                    }
                    else
                    {
                        isDefending = true;
                        Console.WriteLine("Вы закрылись щитом.");
                        if (_random.Next(0, 100) < 40)
                        {
                            playerEvaded = true;
                        }
                    }
                }
                if (!enemy.IsAlive)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(enemy.Name + " повержен.");
                    Console.ResetColor();
                    break;
                }
                if (playerEvaded)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Вы уклонились от удара.");
                    Console.ResetColor();
                    continue;
                }
                int enemyFinalDamage = enemy.AttackPower;
                bool isCrit = false;
                if (enemy.Type == EnemyType.Goblin)
                {
                    int critChance = 67;
                    if (enemy.Name.Contains("ВВГ"))
                    {
                        critChance = 76;
                    }
                    if (_random.Next(0, 100) < critChance)
                    {
                        enemyFinalDamage = (int)(enemyFinalDamage * 1.5);
                        isCrit = true;
                    }
                }
                if (enemy.Type == EnemyType.Skeleton)
                {
                    Console.WriteLine("Удар игнорирует броню!");
                    Console.ResetColor();
                }
                else
                {
                    if (isDefending)
                    {
                        double blockPercent = _random.Next(70, 101) / 100.0;
                        int blockedDamage = (int)(_player.DefensePower * blockPercent);
                        enemyFinalDamage = enemyFinalDamage - blockedDamage;
                        if (enemyFinalDamage < 0)
                        {
                            enemyFinalDamage = 0;
                        }
                    }
                    else
                    {
                        enemyFinalDamage = enemyFinalDamage - _player.DefensePower;
                        if (enemyFinalDamage < 0)
                        {
                            enemyFinalDamage = 0;
                        }
                    }
                }
                if (isCrit)
                {
                    Console.WriteLine("Крит! Получено урона: " + enemyFinalDamage);
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine("Получено урона: " + enemyFinalDamage);
                }
                _player.TakeDamage(enemyFinalDamage);
                if (_player.IsAlive)
                {
                    int freezeChance = 0;
                    if (enemy.Type == EnemyType.Mage)
                    {
                        if (enemy.Name.Contains("Архимаг"))
                        {
                            freezeChance = 50;
                        }
                        else
                        {
                            freezeChance = 35;
                        }
                    }
                    else if (enemy.Name.Contains("Пестов С--"))
                    {
                        freezeChance = 67;
                    }
                    if (freezeChance > 0 && _random.Next(0, 100) < freezeChance)
                    {
                        _player.IsFrozen = true;
                        Console.WriteLine("Вас обморозили.");
                        Console.ResetColor();
                    }
                }
            }
        }
        private PlayerChoice GetPlayerChoice(string chance)
        {
            while (true)
            {
                Console.WriteLine(chance);
                Console.Write("> ");
                string input = Console.ReadLine();
                if (input == "1") return PlayerChoice.Attack;
                if (input == "2") return PlayerChoice.Defend;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Чуть0чутька Не то.");
                Console.ResetColor();
            }
        }
        private ChestChoice GetChestChoice(string chance)
        {
            while (true)
            {
                Console.WriteLine(chance);
                Console.Write("> ");
                string input = Console.ReadLine();
                if (input == "1") return ChestChoice.Equip;
                if (input == "2") return ChestChoice.Discard;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("НемногА не то.");
                Console.ResetColor();
            }
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("НАЧИНАЕМ НАШИ ЛУДОПРИКЛЮЧЕНИЯ");
            string name = "";
            while (string.IsNullOrWhiteSpace(name))
            {
                Console.Write("Имя героя: ");
                name = Console.ReadLine();
            }
            GameEngine game = new GameEngine(name.Trim());
            game.Start();
            Console.WriteLine("\nВыход с ЛудоПриключений... ");
            Console.ReadKey();
        }
    }
}