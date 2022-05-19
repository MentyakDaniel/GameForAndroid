using ditto.mono;
using GameForAndroid.Elements.Gameplay;
using GameForAndroid.Elements.Gameplay.Bosses;
using Microsoft.Xna.Framework.Graphics;

namespace GameForAndroid.Levels
{
    public enum LevelsName
    {
        Prologie,
        Level_1,
        Level_2,
        Level_3,
        Level_4,
        Level_5,
        Level_6,
        Level_7,
        Level_8,
        Level_9,
        Level_10,
        Epilogie,
        EndlessLevel
    }

    public static class Level
    {
        public static LevelsName CurrentLevel = LevelsName.Prologie;
        public static bool StartPlay;
        public static string CurrentBossName => CurrentLevel switch
        {
            LevelsName.Prologie => "PrologieBoss",
            LevelsName.Level_1 => "Level_1_Boss",
            LevelsName.Level_2 => "Level_2_Boss",
            LevelsName.Level_3 => "Level_3_Boss",
            LevelsName.Level_4 => "Level_4_Boss",
            LevelsName.Level_5 => "Level_5_Boss",
            LevelsName.Level_6 => "Level_6_Boss",
            LevelsName.Level_7 => "Level_7_Boss",
            LevelsName.Level_8 => "Level_8_Boss",
            LevelsName.Level_9 => "Level_9_Boss",
            LevelsName.Level_10 => "Level_10_Boss",
            LevelsName.Epilogie => "Epilog_Boss",
            _ => "PrologieBoss",
        };

        public static int NeededScore => CurrentLevel switch
        {
            LevelsName.Prologie => 20,
            LevelsName.Level_1 => 50,
            LevelsName.Level_2 => 100,
            LevelsName.Level_3 => 150,
            LevelsName.Level_4 => 200,
            LevelsName.Level_5 => 250,
            LevelsName.Level_6 => 300,
            LevelsName.Level_7 => 350,
            LevelsName.Level_8 => 400,
            LevelsName.Level_9 => 450,
            LevelsName.Level_10 => 500,
            LevelsName.Epilogie => 550,
            _ => 0,
        };

        public static EnemyShip CurrentBoss(Texture2D bossTexture)
        {
            Boss boss = null;

            switch (CurrentLevel)
            {
                case LevelsName.Prologie:
                    boss = new PrologieBoss(bossTexture);
                    boss.ChangeHP(NeededScore);
                    return boss;
                case LevelsName.Level_1:
                    boss = new Level1Boss(bossTexture);
                    boss.ChangeHP(NeededScore);
                    return boss;
                case LevelsName.Level_2:
                    boss = new Level2Boss(bossTexture);
                    boss.ChangeHP(NeededScore);
                    return boss;
                case LevelsName.Level_3:
                    boss = new Level3Boss(bossTexture);
                    boss.ChangeHP(NeededScore);
                    return boss;
                case LevelsName.Level_4:
                    boss = new Level4Boss(bossTexture);
                    boss.ChangeHP(NeededScore);
                    return boss;
                case LevelsName.Level_5:
                    boss = new Level5Boss(bossTexture);
                    boss.ChangeHP(NeededScore);
                    return boss;
                case LevelsName.Level_6:
                    boss = new Level6Boss(bossTexture);
                    boss.ChangeHP(NeededScore);
                    return boss;
                case LevelsName.Level_7:
                    boss = new Level7Boss(bossTexture);
                    boss.ChangeHP(NeededScore);
                    return boss;
                case LevelsName.Level_8:
                    boss = new Level8Boss(bossTexture);
                    boss.ChangeHP(NeededScore);
                    return boss;
                case LevelsName.Level_9:
                    boss = new Level9Boss(bossTexture);
                    boss.ChangeHP(NeededScore);
                    return boss;
                case LevelsName.Level_10:
                    boss = new Level10Boss(bossTexture);
                    boss.ChangeHP(NeededScore);
                    return boss;
                case LevelsName.Epilogie:
                    boss = new EpilogBoss(bossTexture);
                    boss.ChangeHP(NeededScore);
                    return boss;
                default:
                    return boss;
            }
        }
    }
}