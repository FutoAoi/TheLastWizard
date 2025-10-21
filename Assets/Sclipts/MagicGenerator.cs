public static class MagicGenerator
{
    private static string _magicID;

    public readonly struct MagicParameters
    {
        public readonly int attackPower;
        public readonly int range;
        public readonly int cooldown;
        public readonly int magicSpeed;
        public MagicParameters(int attackPower, int range, int cooldown, int magicSpeed)
        {
            this.attackPower = attackPower;
            this.range = range;
            this.cooldown = cooldown;
            this.magicSpeed = magicSpeed;
        }
    }

    public static string GetID(MagicParameters parameters)
    {
        _magicID = $"{parameters.attackPower:D3}{parameters.range:D3}{parameters.cooldown}{parameters.magicSpeed}";
        return _magicID;
    }

    public static MagicParameters GetMagicParameters(string id)
    {
        int attackPower = int.Parse(id.Substring(0, 3));
        int range = int.Parse(id.Substring(3, 3));
        int cooldown = int.Parse(id.Substring(6, 3));
        int magicSpeed = int.Parse(id.Substring(9, 3));
        MagicParameters parameters = new MagicParameters(attackPower, range, cooldown, magicSpeed);
        return parameters;
    }
}
