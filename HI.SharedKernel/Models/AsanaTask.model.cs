using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace HI.SharedKernel.Models
{
    public partial class AsanaTaskModel
    {
        [JsonProperty("data")] public AsanaTaskData AsanaTaskData { get; set; }
    }

    public class AsanaTaskData
    {
        [JsonProperty("gid")] public string Gid { get; set; }

        [JsonProperty("assignee")] public AsanaTaskAssignee AsanaTaskAssignee { get; set; }

        [JsonProperty("assignee_status")] public string AssigneeStatus { get; set; }

        [JsonProperty("completed")] public bool Completed { get; set; }

        [JsonProperty("completed_at")] public object CompletedAt { get; set; }

        [JsonProperty("created_at")] public DateTimeOffset CreatedAt { get; set; }

        [JsonProperty("custom_fields")] public List<AsanaTaskCustomField> CustomFields { get; set; }

        [JsonProperty("due_at")] public object DueAt { get; set; }

        [JsonProperty("due_on")] public DateTimeOffset DueOn { get; set; }

        [JsonProperty("followers")] public List<AsanaTaskAssignee> Followers { get; set; }

        [JsonProperty("hearted")] public bool Hearted { get; set; }

        [JsonProperty("hearts")] public List<object> Hearts { get; set; }

        [JsonProperty("liked")] public bool Liked { get; set; }

        [JsonProperty("likes")] public List<object> Likes { get; set; }

        [JsonProperty("memberships")] public List<AsanaTaskMembership> Memberships { get; set; }

        [JsonProperty("modified_at")] public DateTimeOffset ModifiedAt { get; set; }

        [JsonProperty("name")] public string Name { get; set; }

        [JsonProperty("notes")] public string Notes { get; set; }

        [JsonProperty("num_hearts")] public long NumHearts { get; set; }

        [JsonProperty("num_likes")] public long NumLikes { get; set; }

        [JsonProperty("parent")] public object Parent { get; set; }

        [JsonProperty("projects")] public List<AsanaTaskAssignee> Projects { get; set; }

        [JsonProperty("resource_type")] public string ResourceType { get; set; }

        [JsonProperty("start_on")] public object StartOn { get; set; }

        [JsonProperty("tags")] public List<object> Tags { get; set; }

        [JsonProperty("resource_subtype")] public string ResourceSubtype { get; set; }

        [JsonProperty("workspace")] public AsanaTaskAssignee Workspace { get; set; }
    }

    public class AsanaTaskAssignee
    {
        [JsonProperty("gid")] public string Gid { get; set; }

        [JsonProperty("name")] public string Name { get; set; }

        [JsonProperty("resource_type")] public string ResourceType { get; set; }
    }

    public class AsanaTaskCustomField
    {
        [JsonProperty("gid")] public string Gid { get; set; }

        [JsonProperty("enabled")] public bool Enabled { get; set; }

        [JsonProperty("name")] public string Name { get; set; }

        [JsonProperty("number_value")] public object NumberValue { get; set; }

        [JsonProperty("precision", NullValueHandling = NullValueHandling.Ignore)]
        public long? Precision { get; set; }

        [JsonProperty("resource_subtype")] public string ResourceSubtype { get; set; }

        [JsonProperty("resource_type")] public string ResourceType { get; set; }

        [JsonProperty("type")] public string Type { get; set; }

        [JsonProperty("enum_options", NullValueHandling = NullValueHandling.Ignore)]
        public List<AsanaTaskEnumOption> EnumOptions { get; set; }

        [JsonProperty("enum_value")] public object EnumValue { get; set; }
    }

    public class AsanaTaskEnumOption
    {
        [JsonProperty("gid")] public string Gid { get; set; }

        [JsonProperty("color")] public string Color { get; set; }

        [JsonProperty("enabled")] public bool Enabled { get; set; }

        [JsonProperty("name")] public string Name { get; set; }

        [JsonProperty("resource_type")] public string ResourceType { get; set; }
    }

    public class AsanaTaskMembership
    {
        [JsonProperty("project")] public AsanaTaskAssignee Project { get; set; }

        [JsonProperty("section")] public AsanaTaskAssignee Section { get; set; }
    }

    public partial class AsanaTaskModel
    {
        public static AsanaTaskModel FromJson(string json) =>
            JsonConvert.DeserializeObject<AsanaTaskModel>(json, AsanaTaskModelConverter.Settings);
    }

    public static class AsanaTaskSerialize
    {
        public static string ToJson(this AsanaTaskModel self) =>
            JsonConvert.SerializeObject(self, AsanaTaskModelConverter.Settings);
    }

    internal static class AsanaTaskModelConverter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter {DateTimeStyles = DateTimeStyles.AssumeUniversal}
            },
        };
    }
}