using System;

public interface IAchievements 
{
    void UnlockAchievement( string achievement_name );
    void SetStatistic( string stat_name, int stat_value );
    void SetStatistic( string stat_name, float stat_value );
}
