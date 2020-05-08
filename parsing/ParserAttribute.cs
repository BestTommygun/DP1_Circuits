using System;

[System.AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
sealed class ParserAttribute : Attribute
{
    public string Name { get; }

    public ParserAttribute(string name)
    {
        this.Name = name;
    }
}
