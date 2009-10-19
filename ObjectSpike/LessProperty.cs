namespace ObjectSpike
{
    internal class LessProperty
    {
        public string Key;
        public string Value;

        public string ToCss()
        {
            return string.Format("{0}: {1};", Key, Value);
        }
    }
}