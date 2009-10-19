using System;

namespace ObjectSpike
{
    [System.Diagnostics.DebuggerDisplay("Selector:{Name}")]
    class LessSelector : IEquatable<LessSelector>
    {
        public string Name { get; set; }

        public bool Equals(LessSelector other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.Name, Name);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (LessSelector)) return false;
            return Equals((LessSelector) obj);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public static bool operator ==(LessSelector left, LessSelector right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(LessSelector left, LessSelector right)
        {
            return !Equals(left, right);
        }
    }
}