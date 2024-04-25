using Exiled.API.Interfaces;
using System;

namespace ItemGlow;

public class Config : IConfig
{
    public bool IsEnabled { get; set; } = true;
    public bool Debug { get; set; } = false;
}