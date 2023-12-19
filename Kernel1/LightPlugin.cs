#region HEADER
// /**
//  * Original author:  Krzysztof Kasprowicz
//  * Created:  14.12.2023
//  *
//  * Copyright © 2023 IMPE KASPROWICZ KRZYSZTOF
//  **/
#endregion

using System.ComponentModel;
using Microsoft.SemanticKernel;

namespace Kernel1;

public class LightPlugin
{
    public bool IsOn { get; set; } = false;

    [KernelFunction]
    [Description("Gets the state of the light.")]
    public string GetState() => this.IsOn ? "on" : "off";

    [KernelFunction]
    [Description("Changes the state of the light.'")]
    public string ChangeState(bool newState)
    {
        this.IsOn = newState;
        var state = this.GetState();

        // Print the state to the console
        Console.ForegroundColor = ConsoleColor.DarkBlue;
        Console.WriteLine($"[Light is now {state}]");
        Console.ResetColor();

        return state;
    }
}