// ReSharper disable InconsistentNaming

namespace Soul.Model.Runtime
{
    public static class Constant
    {
        public static class User
        {
            public static readonly string KEY_FIRST_INSTALL_DATE = "user_first_install_date";
            public const string KEY_LANGUAGE = "user_lang";
            public const string KEY_QUALITY = "user_quality";
            public const string KEY_MUSIC = "user_music";
            public const string KEY_SFX = "user_sfx";
            public const string KEY_VIBRATE = "user_vibrate";
            public const string KEY_FIRST_OPEN = "user_first_open";
            public const string KEY_ID = "user_id";
            public const string AGREE_PRIVACY = "user_agree_privacy";

            public static class DailyReward
            {
                public const string CURRENT_WEEK = "user_current_week_dr";
                public const string CURRENT_DAY = "user_current_day_dr";
                public const string LAST_TIME_UPDATE = "user_last_time_update_dr";
                public const string ALL_DAY_CLAIMED_IN_WEEK = "user_all_day_claimed_dr";
            }
        }

        public static class Scene
        {
            public static readonly string Persistent = "persistent";
            public static readonly string Menu = "ui_cam";
            public static readonly string Infrastructure = "farming_infrastructure";
            public static readonly string Environment = "environment";
            public static readonly string NPC = "npc";
        }
    }
}