using System;
using System.Collections.Generic;

namespace ConsoleRogueLike
{
    #region Перечисления (Enums)
    public enum EnemyType { Goblin, Skeleton, Mage }
    public enum PlayerChoice { Attack = 1, Defend = 2 }
    public enum ChestChoice { Equip = 1, Discard = 2 }
    #endregion

    #region Экипировка и Предметы
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
        public int Damage => Value;
        public Weapon(string name, int damage) : base(name, damage) { }
    }

    public class Armor : Item
    {
        public int Defense => Value;
        public Armor(string name, int defense) : base(name, defense) { }
    }
    #endregion

    #region Сущности (Entities)
    public abstract class Entity
    {
        public string Name { get; protected set; }
        public int MaxHp { get; protected set; }
        public int CurrentHp { get; protected set; }
        public abstract int AttackPower { get; }
        public abstract int DefensePower { get; }

        public bool IsAlive => CurrentHp > 0;

        protected Entity(string name, int maxHp)
        {
            Name = name;
            MaxHp = maxHp;
            CurrentHp = maxHp;
        }

        public virtual void TakeDamage(int damage)
        {
            int actualDamage = Math.Max(0, damage);
            CurrentHp = Math.Max(0, CurrentHp - actualDamage);
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

        public override int AttackPower => CurrentWeapon != null ? CurrentWeapon.Damage : 5;
        public override int DefensePower => CurrentArmor != null ? CurrentArmor.Defense : 0;

        public Player(string name) : base(name, 100)
        {
            CurrentWeapon = new Weapon("Ржавый кинжал", 10);
            CurrentArmor = new Armor("Обноски", 5);
            IsFrozen = false;
        }

        public void ChangeWeapon(Weapon newWeapon) => CurrentWeapon = newWeapon;
        public void ChangeArmor(Armor newArmor) => CurrentArmor = newArmor;
    }

    public class Enemy : Entity
    {
        protected int baseAttack;
        protected int baseDefense;

        public EnemyType Type { get; }
        public override int AttackPower => baseAttack;
        public override int DefensePower => baseDefense;

        public Enemy(string name, int hp, int attack, int defense, EnemyType type) : base(name, hp)
        {
            baseAttack = attack;
            baseDefense = defense;
            Type = type;
        }
    }
    #endregion

    #region Логика Игры
    public class GameEngine
    {
        private readonly Player _player;
        private readonly Random _random = new Random();
        private int _turnCounter = 0;

        private readonly List<string> _weaponNames = new List<string> { "Стальной меч", "Топор палача", "Копьё судьбы", "Лук теней", "Молот правосудия" };
        private readonly List<string> _armorNames = new List<string> { "Кожаный жилет", "Кольчуга", "Рыцарские латы", "Панцирь титана", "Мантия духов" };

        public GameEngine(string playerName)
        {
            _player = new Player(playerName);
        }

        public void Start()
        {
            Console.Clear();
            Console.WriteLine("Добро пожаловать в мрачные подземелья, " + _player.Name + "!");
            Console.WriteLine("Нажмите любую клавишу, чтобы сделать первый шаг...");
            Console.ReadKey();

            while (_player.IsAlive)
            {
                _turnCounter++;
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("=== ХОД НОМЕР " + _turnCounter + " ===");
                Console.ResetColor();
                Console.WriteLine("Ваше состояние: HP: " + _player.CurrentHp + "/" + _player.MaxHp + " | Оружие: " + _player.CurrentWeapon.Name + " (" + _player.AttackPower + " DMG) | Броня: " + _player.CurrentArmor.Name + " (" + _player.DefensePower + " DEF)\n");

                if (_turnCounter % 10 == 0)
                {
                    Enemy boss = GenerateBoss();
                    ExecuteCombat(boss);
                }
                else
                {
                    if (_random.Next(0, 2) == 0)
                    {
                        ExecuteChestEvent();
                    }
                    else
                    {
                        Enemy enemy = GenerateRandomEnemy();
                        ExecuteCombat(enemy);
                    }
                }

                if (_player.IsAlive)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("\nНажмите любую клавишу для перехода к следующему ходу...");
                    Console.ResetColor();
                    Console.ReadKey();
                }
            }

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("ИГРА ОКОНЧЕНА. ВЫ ПОГИБЛИ!");
            Console.WriteLine("Вы продержались ходов: " + (_turnCounter - 1));
            Console.ResetColor();
        }

        private Enemy GenerateRandomEnemy()
        {
            int typeIndex = _random.Next(0, 3);
            EnemyType type = (EnemyType)typeIndex;

            switch (type)
            {
                case EnemyType.Goblin:
                    return new Enemy("Дикий Гоблин", _random.Next(30, 50), _random.Next(12, 18), _random.Next(4, 8), type);
                case EnemyType.Skeleton:
                    return new Enemy("Костлявый Скелет", _random.Next(25, 45), _random.Next(15, 22), _random.Next(2, 5), type);
                case EnemyType.Mage:
                    return new Enemy("Тёмный Маг", _random.Next(20, 40), _random.Next(18, 25), _random.Next(1, 4), type);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private Enemy GenerateBoss()
        {
            int bossIndex = _random.Next(0, 4);
            switch (bossIndex)
            {
                case 0:
                    return new Enemy("ВВГ (Босс-Гоблин)", (int)(45 * 2.0), (int)(15 * 1.5), (int)(6 * 1.2), EnemyType.Goblin);
                case 1:
                    return new Enemy("Ковальский (Босс-Скелет)", (int)(35 * 2.5), (int)(18 * 1.3), (int)(3 * 1.4), EnemyType.Skeleton);
                case 2:
                    return new Enemy("Архимаг C++ (Босс-Маг)", (int)(30 * 1.8), (int)(22 * 1.6), (int)(2 * 1.1), EnemyType.Mage);
                default:
                    return new Enemy("Пестов С-- (Босс-Скелет)", (int)(35 * 1.3), (int)(18 * 1.8), (int)(3 * 0.6), EnemyType.Skeleton);
            }
        }

        private void ExecuteChestEvent()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("[СУНДУК] Вы нашли таинственный сундук!");
            Console.ResetColor();

            int lootType = _random.Next(0, 3);

            if (lootType == 0)
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("[ЗЕЛЬЕ] Внутри оказалось Лечебное Зелье! Ваше здоровье полностью восстановлено.");
                Console.ResetColor();
                _player.HealFull();
            }
            else if (lootType == 1)
            {
                Weapon newWeapon = new Weapon(_weaponNames[_random.Next(_weaponNames.Count)], _random.Next(12, 35));
                Console.WriteLine("[ОРУЖИЕ] Вы нашли оружие: " + newWeapon.Name + " (Урон: " + newWeapon.Damage + ")");
                Console.WriteLine("Текущее оружие: " + _player.CurrentWeapon.Name + " (Урон: " + _player.AttackPower + ")");

                if (GetChestChoice("Вы выберете:\n1. Экипировать новое оружие\n2. Выбросить его") == ChestChoice.Equip)
                {
                    _player.ChangeWeapon(newWeapon);
                    Console.WriteLine("Вы экипировали новое оружие!");
                }
            }
            else
            {
                Armor newArmor = new Armor(_armorNames[_random.Next(_armorNames.Count)], _random.Next(6, 25));
                Console.WriteLine("[ДОСПЕХ] Вы нашли доспех: " + newArmor.Name + " (Защита: " + newArmor.Defense + ")");
                Console.WriteLine("Текущий доспех: " + _player.CurrentArmor.Name + " (Защита: " + _player.DefensePower + ")");

                if (GetChestChoice("Вы выберете:\n1. Экипировать новый доспех\n2. Выбросить его") == ChestChoice.Equip)
                {
                    _player.ChangeArmor(newArmor);
                    Console.WriteLine("Вы надели новый доспех!");
                }
            }
        }

        private void ExecuteCombat(Enemy enemy)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("[ВНИМАНИЕ] На вас напал " + enemy.Name + "!");
            Console.ResetColor();
            Console.WriteLine("Характеристики врага: HP: " + enemy.CurrentHp + " | ATK: " + enemy.AttackPower + " | DEF: " + enemy.DefensePower + "\n");

            while (_player.IsAlive && enemy.IsAlive)
            {
                bool playerEvaded = false;
                bool isDefending = false;

                if (_player.IsFrozen)
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("[ЗАМОРОЗКА] Ловушка времени! " + _player.Name + " заморожен и пропускает этот ход!");
                    Console.ResetColor();
                    _player.IsFrozen = false;
}
                else
                {
                    Console.WriteLine("\nВаш ход! Ваше HP: " + _player.CurrentHp + ". HP врага: " + enemy.CurrentHp);
                    PlayerChoice choice = GetPlayerChoice("Выберите действие:\n1. Атака\n2. Защита");
                    if (choice == PlayerChoice.Attack)
                    {
                        int dmgToEnemy = Math.Max(1, _player.AttackPower - enemy.DefensePower);
                        enemy.TakeDamage(dmgToEnemy);
                        Console.WriteLine("Вы нанесли " + enemy.Name + " " + dmgToEnemy + " урона!");
                    }
                    else
                    {
                        isDefending = true;
                        Console.WriteLine("Вы приготовились к защите (Шанс уклонения 40%, увеличенный блок).");
                        if (_random.Next(0, 100) < 40)
                        {
                            playerEvaded = true;
                        }
                    }
                }
                if (!enemy.IsAlive)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("[ПОБЕДА] " + enemy.Name + " повержен!");
                    Console.ResetColor();
                    break;
                }
                Console.WriteLine("\nХодит " + enemy.Name + ":");
                if (playerEvaded)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Вы полностью уклонились от атаки " + enemy.Name + "!");
                    Console.ResetColor();
                    continue;
                }
                int enemyFinalDamage = enemy.AttackPower;
                bool isCrit = false;
                if (enemy.Type == EnemyType.Goblin)
                {
                    int critChance = (enemy.Name.Contains("Босс")) ? 25 : 15;
                    if (_random.Next(0, 100) < critChance)
                    {
                        enemyFinalDamage = (int)(enemyFinalDamage * 1.5);
                        isCrit = true;
                    }
                }
                if (enemy.Type == EnemyType.Skeleton)
                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("Атака " + enemy.Name + " полностью игнорирует вашу броню!");
                    Console.ResetColor();
                }
                else
                {
                    if (isDefending)
                    {
                        double blockPercent = _random.Next(70, 101) / 100.0;
                        int blockedDamage = (int)(_player.DefensePower * blockPercent);
                        enemyFinalDamage = Math.Max(0, enemyFinalDamage - blockedDamage);
                        Console.WriteLine("Сработал блок! Вы снизили входящий урон на " + blockedDamage + " благодаря броне.");
                    }
                    else
                    {
                        enemyFinalDamage = Math.Max(0, enemyFinalDamage - _player.DefensePower);
                    }
                }
                if (isCrit)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("[КРИТИЧЕСКИЙ УДАР] " + enemy.Name + " наносит вам " + enemyFinalDamage + " урона!");
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine(enemy.Name + " наносит вам " + enemyFinalDamage + " урона.");
                }
                _player.TakeDamage(enemyFinalDamage);
                if (_player.IsAlive)
                {
                    int freezeChance = 0;
                    if (enemy.Type == EnemyType.Mage)
                    {
                        freezeChance = enemy.Name.Contains("Архимаг") ? 25 : 15;
                    }
                    else if (enemy.Name.Contains("Пестов С--"))
                    {
                        freezeChance = 30;
                    }
                    if (freezeChance > 0 && _random.Next(0, 100) < freezeChance)
                    {
                        _player.IsFrozen = true;
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine("[ЗАМОРОЗКА] " + enemy.Name + " наложил на вас эффект ЗАМОРОЗКИ!");
                        Console.ResetColor();
                    }
                }
            }
        }
        // Прямые методы ввода без использования Generics (поддерживаются всеми версиями C#)
        private PlayerChoice GetPlayerChoice(string prompt)
        {
            while (true)
            {
                Console.WriteLine(prompt);
                Console.Write("Введите номер действия: ");
                string input = Console.ReadLine();
                if (input == "1") return PlayerChoice.Attack;
                if (input == "2") return PlayerChoice.Defend;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[ОШИБКА] Некорректный ввод. Пожалуйста, введите 1 или 2.");
                Console.ResetColor();
                Console.WriteLine();
            }
        }
        private ChestChoice GetChestChoice(string prompt)
        {
            while (true)
            {
                Console.WriteLine(prompt);
                Console.Write("Введите номер действия: ");
                string input = Console.ReadLine();
                if (input == "1") return ChestChoice.Equip;
                if (input == "2") return ChestChoice.Discard;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[ОШИБКА] Некорректный ввод. Пожалуйста, введите 1 или 2.");
                Console.ResetColor();
                Console.WriteLine();
            }
        }
    }
    #endregion
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("========================================");
            Console.WriteLine("ДОБРО ПОЖАЛОВАТЬ В ПОШАГОВЫЙ РОГАЛИК");
            Console.WriteLine("========================================");
            string name = "";
            while (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(name.Trim()))
            {
                Console.Write("Введите имя вашего героя: ");
                name = Console.ReadLine();
            }
            GameEngine game = new GameEngine(name);
            game.Start();
            Console.WriteLine("\nНажмите любую клавишу для выхода из программы...");
            Console.ReadKey();
        }
    }
}