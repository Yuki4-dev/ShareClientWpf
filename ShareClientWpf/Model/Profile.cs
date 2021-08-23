using System.Text.Json;

namespace ShareClientWpf
{
    public class Profile : ModelBase
    {
        private byte[] iconImage;
        public byte[] IconImage
        {
            get => iconImage;
            set => SetProperty(ref iconImage, value);
        }

        private string name;
        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }

        public string GetJsonString()
        {
            var josn = new ProfileJson()
            {
                IconImage = this.IconImage,
                Name = this.Name
            };
            return JsonSerializer.Serialize(josn);
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
