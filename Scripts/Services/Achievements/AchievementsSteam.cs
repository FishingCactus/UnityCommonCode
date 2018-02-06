#if STEAM_VERSION
using Steamworks;

public class AchievementsSteam : IAchievements
{
    public void UnlockAchievement( string achievement_name )
    {
        if ( SteamManager.Initialized )
        {
            SteamUserStats.SetAchievement( achievement_name );
            SteamUserStats.StoreStats();
        }
    }

    public void SetStatistic( string stat_name, int stat_value )
    {
        if ( SteamManager.Initialized )
        {
            SteamUserStats.SetStat( stat_name, stat_value );
        }
    }

    public void SetStatistic( string stat_name, float stat_value )
    {
        if ( SteamManager.Initialized )
        {
            SteamUserStats.SetStat( stat_name, stat_value );
        }
    }
}
#endif
