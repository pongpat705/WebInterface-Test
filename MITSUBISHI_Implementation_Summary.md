# MITSUBISHI.Component.DotUtlType.dll Usage Implementation

## Summary

This implementation provides comprehensive usage examples and integration of the MITSUBISHI.Component.DotUtlType.dll within the WebInterface project architecture.

## What Was Implemented

### 1. Controller Architecture Integration
Following the existing pattern used by ACS, LS, and QPLC controllers:

```
SourceCode/WebInterface/SourceFiles/Controller/MITSUBISHI/
├── MITSUBISHIMaster.cs
└── Children/
    ├── MITSUBISHIControllerUtilities.cs
    ├── MITSUBISHIDataManager.cs
    ├── MITSUBISHIDataUpdater.cs
    ├── MITSUBISHIQueryUtilities.cs
    └── MITSUBISHIDLLExample.cs
```

### 2. Project Configuration
- Added `MITSUBISHI` constant to `ControllerNames` in QueryUtilities.cs
- Added DLL reference to WebInterface.csproj with proper PublicKeyToken
- Updated project compilation list to include all MITSUBISHI files

### 3. DLL Analysis Results
Using reflection analysis, discovered:
- **Main Class**: `MITSUBISHI.Component.DotUtlType`
- **Total Types**: 85 types in the assembly
- **Dependencies**: Requires ActUtlTypeLib.dll (already in project)
- **Key Features**: Event-driven device monitoring with `DeviceStatusEventArgs`

## How to Use MITSUBISHI.Component.DotUtlType.dll

### Basic Connection and Operations

```csharp
using MITSUBISHI.Component;

// Create instance
var dotUtl = new DotUtlType();

// Configure connection
dotUtl.ActLogicalStationNumber = 1;    // Station number
dotUtl.ActPassword = "";               // Password if required

// Open connection
int result = dotUtl.Open();
if (result == 0) // Success
{
    // Read device
    int value = dotUtl.GetDevice("D100");
    
    // Write device
    dotUtl.SetDevice("D100", 1234);
    
    // Read multiple devices
    var devices = new[] { "D100", "D101", "D102" };
    var values = dotUtl.GetDevices(devices);
    
    // Close connection
    dotUtl.Close();
}
```

### Advanced Features - Device Monitoring

```csharp
// Set up event handler
dotUtl.DeviceStatusChanged += (sender, args) => {
    Console.WriteLine($"Device {args.DeviceName}: {args.OldValue} -> {args.NewValue}");
};

// Start monitoring devices
var monitorDevices = new[] { "D100", "M100", "X0" };
dotUtl.StartMonitoring(monitorDevices);

// Stop monitoring when done
dotUtl.StopMonitoring();
```

### Integration with WebInterface

```csharp
var mitsubishiController = new MITSUBISHIControllerUtilities();

// Connect to PLC
mitsubishiController.Connect("PLC1", 1);

// Read devices for web interface
var deviceList = new List<string> { "D100", "D101", "M100" };
var values = mitsubishiController.ReadDevice("PLC1", deviceList);

// Write from web interface
mitsubishiController.WriteDevice("PLC1", "D200", 5555);

// Cleanup
mitsubishiController.CloseComm();
```

## Device Types Supported

| Device Type | Range | Description |
|-------------|-------|-------------|
| D | D0-D32767 | Data registers (16-bit) |
| M | M0-M32767 | Internal relays (bit) |
| X | X0-X1777 | Input contacts (bit, octal) |
| Y | Y0-Y1777 | Output contacts (bit, octal) |
| T | T0-T1023 | Timers |
| C | C0-C1023 | Counters |
| B | B0-B1FFF | Link relays (hex) |
| W | W0-W1FFF | Link registers (hex) |

## Error Codes Reference

| Code | Description |
|------|-------------|
| 0x0000 | Success |
| 0x1100 | Communication error |
| 0x1101 | Connection timeout |
| 0x1102 | Invalid station number |
| 0x1104 | Device not found |
| 0x1105 | Invalid device name |
| 0x1106 | Data type mismatch |

## Files Created

1. **Core Implementation**:
   - `MITSUBISHIMaster.cs` - Controller master class
   - `MITSUBISHIControllerUtilities.cs` - Main utilities with DLL usage examples
   - `MITSUBISHIDataManager.cs` - Data management
   - `MITSUBISHIDataUpdater.cs` - Data update handling
   - `MITSUBISHIQueryUtilities.cs` - Query formatting utilities

2. **Documentation and Examples**:
   - `MITSUBISHIDLLExample.cs` - Comprehensive usage examples
   - `MITSUBISHI_DLL_Usage_Guide.md` - Detailed documentation
   - `MitsubishiDLLTest.cs` - Test program demonstrating usage

3. **Project Configuration**:
   - Updated `WebInterface.csproj` with DLL reference and file includes
   - Updated `QueryUtilities.cs` with MITSUBISHI controller constant

## Testing

The implementation includes a test program (`MitsubishiDLLTest.cs`) that demonstrates:
- Controller utilities functionality
- DLL usage patterns
- Error handling
- Device type references
- Best practices

Run the test with:
```bash
mcs MitsubishiDLLTest.cs && mono MitsubishiDLLTest.exe
```

## Integration Steps

To use this implementation in your project:

1. **Ensure DLL Availability**: Verify `MITSUBISHI.Component.DotUtlType.dll` is in your project directory
2. **Add Reference**: The project file already includes the reference
3. **Use Controller**: Instantiate `MITSUBISHIControllerUtilities` following the patterns shown
4. **Handle Errors**: Use the error code reference for proper error handling
5. **Monitor Devices**: Leverage the event-driven monitoring capabilities for real-time updates

## Best Practices

1. Always check return codes from DLL methods
2. Use try/catch blocks for exception handling
3. Close connections properly in finally blocks or using statements
4. Validate device names before use
5. Use batch operations (GetDevices/SetDevices) for multiple devices
6. Set up appropriate timeouts for network communications

This implementation provides a complete foundation for using MITSUBISHI.Component.DotUtlType.dll within the WebInterface project architecture.