using CommunityToolkit.Mvvm.ComponentModel;
using System.Text.Json;

namespace ShareClientWpf
{
    [ObservableObject]
    public partial class Profile
    {
        [ObservableProperty]
        private byte[] iconImage;

        [ObservableProperty]
        private string name;

        public string GetJsonString()
        {
            var json = new ProfileJson()
            {
                IconImage = this.IconImage,
                Name = this.Name
            };
            return JsonSerializer.Serialize(json);
        }

        public static Profile FromJson(string jsonString)
        {
            var json = JsonSerializer.Deserialize<ProfileJson>(jsonString);
            return new Profile()
            {
                IconImage = json.IconImage,
                Name = json.Name
            };
        }

        private class ProfileJson
        {
            public byte[] IconImage { get; set; }
            public string Name { get; set; }
        }
    }
}
