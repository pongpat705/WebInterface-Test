# MITSUBISHI.Component.DotUtlType.dll Usage Guide

This document provides comprehensive examples of how to use the MITSUBISHI.Component.DotUtlType.dll within the WebInterface project.

## Overview

The MITSUBISHI.Component.DotUtlType.dll is a .NET assembly that provides communication utilities for Mitsubishi PLCs and controllers. It's similar to ActUtlTypeLib.dll but is a more modern implementation.

## Key Classes and Interfaces

### Primary Classes
- `ActUtlTypeClass` - Main communication class for PLC interaction
- `ActSupportMsgClass` - Support for message handling and diagnostics
- `ActEasyIFClass` - Simplified interface for basic operations

## Basic Usage Patterns

### 1. Adding Reference to Project

Add the following reference to your .csproj file:

```xml
<Reference Include="MITSUBISHI.Component.DotUtlType">
  <HintPath>..\..\MITSUBISHI.Component.DotUtlType.dll</HintPath>
</Reference>
```

### 2. Basic Connection Example

```csharp
using MITSUBISHI.Component.DotUtlType;

// Create instance
var actUtil = new ActUtlTypeClass();

// Configure connection
actUtil.ActLogicalStationNumber = 1;    // PLC station number
actUtil.ActPassword = "";               // Password (if required)
actUtil.ActConnectUnitNumber = 0;       // Unit number

// Open connection
int result = actUtil.Open();
if (result != 0)
{
    throw new Exception($"Connection failed: 0x{result:X}");
}

// Use the connection...

// Close when done
actUtil.Close();
```

### 3. Reading Device Values

#### Single Device Read
```csharp
// Read a single data register
int value;
int result = actUtil.GetDevice("D100", out value);
if (result == 0)
{
    Console.WriteLine($"D100 = {value}");
}
```

#### Multiple Device Read
```csharp
// Read multiple devices at once
string deviceList = "D100\nD101\nD102\nD103";
short[] data = new short[4];
int result = actUtil.ReadDeviceRandom(deviceList, 4, out data[0]);

if (result == 0)
{
    for (int i = 0; i < data.Length; i++)
    {
        Console.WriteLine($"Device {i}: {data[i]}");
    }
}
```

### 4. Writing Device Values

#### Single Device Write
```csharp
// Write to a data register
int result = actUtil.SetDevice("D100", 1234);
if (result != 0)
{
    throw new Exception($"Write failed: 0x{result:X}");
}
```

#### Multiple Device Write
```csharp
// Write multiple devices
string deviceList = "D200\nD201\nD202";
short[] values = { 100, 200, 300 };
int result = actUtil.WriteDeviceRandom(deviceList, 3, ref values[0]);
```

### 5. Working with Different Device Types

#### Data Registers (D devices)
```csharp
// 16-bit signed values (-32768 to 32767)
actUtil.SetDevice("D0", 12345);
actUtil.GetDevice("D0", out int dValue);
```

#### Internal Relays (M devices)
```csharp
// Bit devices (0 = OFF, 1 = ON)
actUtil.SetDevice("M100", 1);  // Turn ON
actUtil.SetDevice("M100", 0);  // Turn OFF
actUtil.GetDevice("M100", out int mValue);
```

#### Input/Output Contacts (X/Y devices)
```csharp
// Read input contacts
actUtil.GetDevice("X0", out int inputValue);

// Write output contacts
actUtil.SetDevice("Y0", 1);  // Turn output ON
actUtil.SetDevice("Y0", 0);  // Turn output OFF
```

#### Timers and Counters
```csharp
// Timer preset value (units depend on timer type)
actUtil.SetDevice("T0", 1000);  // 10 seconds for 100ms timer
actUtil.GetDevice("T0", out int timerValue);

// Counter preset value
actUtil.SetDevice("C0", 100);   // Count to 100
actUtil.GetDevice("C0", out int counterValue);
```

## Integration with WebInterface Architecture

### 1. Controller Structure

The MITSUBISHI controller follows the same pattern as other controllers in the project:

```
Controller/
  MITSUBISHI/
    MITSUBISHIMaster.cs
    Children/
      MITSUBISHIControllerUtilities.cs
      MITSUBISHIDataManager.cs
      MITSUBISHIDataUpdater.cs
      MITSUBISHIQueryUtilities.cs
```

### 2. Usage in MITSUBISHIControllerUtilities

```csharp
public class MITSUBISHIControllerUtilities
{
    private Dictionary<string, ActUtlTypeClass> mitsubishiDevices;

    public void Connect(string controllerName, int stationNumber)
    {
        var actUtil = new ActUtlTypeClass();
        actUtil.ActLogicalStationNumber = stationNumber;
        
        int result = actUtil.Open();
        if (result == 0)
        {
            mitsubishiDevices.Add(controllerName, actUtil);
        }
        else
        {
            throw new Exception($"Connection to {controllerName} failed: 0x{result:X}");
        }
    }

    public List<int> ReadDevices(string controllerName, List<string> deviceList)
    {
        var actUtil = mitsubishiDevices[controllerName];
        string devices = string.Join("\n", deviceList);
        short[] data = new short[deviceList.Count];
        
        int result = actUtil.ReadDeviceRandom(devices, deviceList.Count, out data[0]);
        if (result == 0)
        {
            return data.Select(d => (int)d).ToList();
        }
        else
        {
            throw new Exception($"Read failed: 0x{result:X}");
        }
    }
}
```

## Error Handling

### Common Error Codes
- `0x0000` - Success
- `0x1100` - Communication error
- `0x1101` - Connection timeout
- `0x1102` - Invalid station number
- `0x1104` - Device not found
- `0x1105` - Invalid device name
- `0x1106` - Data type mismatch

### Error Handling Pattern
```csharp
int result = actUtil.GetDevice("D100", out int value);
switch (result)
{
    case 0:
        // Success
        break;
    case 0x1104:
        throw new Exception("Device D100 not found");
    case 0x1105:
        throw new Exception("Invalid device name format");
    default:
        throw new Exception($"Unexpected error: 0x{result:X}");
}
```

## Best Practices

1. **Always check return codes** - All methods return error codes that should be checked
2. **Close connections properly** - Use try/finally or using statements
3. **Handle exceptions gracefully** - Communication can fail for various reasons
4. **Use appropriate device types** - Different devices have different value ranges
5. **Batch operations when possible** - Use ReadDeviceRandom/WriteDeviceRandom for multiple devices

## Example Complete Implementation

```csharp
using MITSUBISHI.Component.DotUtlType;

public class MitsubishiPLCInterface
{
    private ActUtlTypeClass plc;
    
    public void Connect(int stationNumber)
    {
        plc = new ActUtlTypeClass();
        plc.ActLogicalStationNumber = stationNumber;
        
        int result = plc.Open();
        if (result != 0)
        {
            throw new Exception($"PLC connection failed: 0x{result:X}");
        }
    }
    
    public int ReadDataRegister(string device)
    {
        int value;
        int result = plc.GetDevice(device, out value);
        if (result != 0)
        {
            throw new Exception($"Read {device} failed: 0x{result:X}");
        }
        return value;
    }
    
    public void WriteDataRegister(string device, int value)
    {
        int result = plc.SetDevice(device, value);
        if (result != 0)
        {
            throw new Exception($"Write {device} failed: 0x{result:X}");
        }
    }
    
    public void Disconnect()
    {
        if (plc != null)
        {
            plc.Close();
            plc = null;
        }
    }
}
```

This guide provides the foundation for integrating MITSUBISHI.Component.DotUtlType.dll into the WebInterface project following the established architectural patterns.