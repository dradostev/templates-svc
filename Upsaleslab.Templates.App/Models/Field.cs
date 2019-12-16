namespace Upsaleslab.Templates.App.Models
{
    public class Field
    {
        public string Type { get; private set; }

        public string Key { get; private set; }

        public string Value { get; private set; }

        public Field(string type, string key, string value)
        {
            Type = type;
            Key = key;
            Value = value;
        }
    }
}