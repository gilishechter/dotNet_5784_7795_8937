namespace Dal;
internal static class Config
{
    static string s_data_config_xml = "data-config";
    internal static int NextTaskId { get => XMLTools.GetAndIncreaseNextId(s_data_config_xml, "NextTaskId");
                                     set => XMLTools.SetNextId(s_data_config_xml, "NextTaskId", value);  }
    internal static int NextDependenceId { get => XMLTools.GetAndIncreaseNextId(s_data_config_xml, "NextDependenceId");
                                           set => XMLTools.SetNextId(s_data_config_xml, "NextDependenceId", value);    }

}
