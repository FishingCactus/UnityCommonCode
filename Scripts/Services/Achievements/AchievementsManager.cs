﻿using System;

public class AchievementsManager : FishingCactus.Singleton<AchievementsManager>
{
    IAchievements AchievementEngine;

    public void UnlockAchievement( string achievement_name )
    {
        AchievementEngine.UnlockAchievement( achievement_name );
    }

    public void SetStatistic( string stat_name, int stat_value )
    {
        AchievementEngine.SetStatistic( stat_name, stat_value );
    }

    public void SetStatistic( string stat_name, float stat_value )
    {
        AchievementEngine.SetStatistic( stat_name, stat_value );
    }
    
    public override void Awake()
    {
        base.Awake();

#if STEAM_VERSION
        AchievementEngine = new AchievementsSteam();
#endif

    }
}