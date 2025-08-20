using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebInterface
{
    /// <summary>
    /// Working example demonstrating actual MITSUBISHI.Component.DotUtlType.dll usage
    /// This class shows practical implementation patterns for the DLL
    /// </summary>
    public static class MITSUBISHIDLLExample
    {
        /// <summary>
        /// Example showing how to use the DLL with proper error handling and connection management
        /// </summary>
        public static void DemonstrateBasicUsage()
        {
            /*
            // Uncomment and modify when adding the DLL reference:
            
            try
            {
                // Step 1: Create the communication object
                var actUtil = new MITSUBISHI.Component.DotUtlType.ActUtlTypeClass();
                
                // Step 2: Configure connection parameters
                actUtil.ActLogicalStationNumber = 1;    // Station number (1-32)
                actUtil.ActPassword = "";               // Password if required
                actUtil.ActConnectUnitNumber = 0;       // Unit number (usually 0)
                
                // Step 3: Establish connection
                int openResult = actUtil.Open();
                if (openResult != 0)
                {
                    Console.WriteLine($"Connection failed: Error code 0x{openResult:X}");
                    return;
                }
                
                Console.WriteLine("Connected to MITSUBISHI PLC successfully!");
                
                // Step 4: Read single device
                int deviceValue;
                int readResult = actUtil.GetDevice("D100", out deviceValue);
                if (readResult == 0)
                {
                    Console.WriteLine($"D100 current value: {deviceValue}");
                }
                else
                {
                    Console.WriteLine($"Failed to read D100: Error 0x{readResult:X}");
                }
                
                // Step 5: Write single device
                int writeResult = actUtil.SetDevice("D100", 12345);
                if (writeResult == 0)
                {
                    Console.WriteLine("Successfully wrote 12345 to D100");
                }
                else
                {
                    Console.WriteLine($"Failed to write D100: Error 0x{writeResult:X}");
                }
                
                // Step 6: Read multiple devices at once
                string deviceList = "D100\nD101\nD102\nD103\nD104";
                short[] dataArray = new short[5];
                int multiReadResult = actUtil.ReadDeviceRandom(deviceList, 5, out dataArray[0]);
                
                if (multiReadResult == 0)
                {
                    Console.WriteLine("Multiple device read successful:");
                    for (int i = 0; i < dataArray.Length; i++)
                    {
                        Console.WriteLine($"  D{100 + i} = {dataArray[i]}");
                    }
                }
                else
                {
                    Console.WriteLine($"Multiple read failed: Error 0x{multiReadResult:X}");
                }
                
                // Step 7: Write multiple devices
                string writeDeviceList = "D200\nD201\nD202";
                short[] writeData = { 1000, 2000, 3000 };
                int multiWriteResult = actUtil.WriteDeviceRandom(writeDeviceList, 3, ref writeData[0]);
                
                if (multiWriteResult == 0)
                {
                    Console.WriteLine("Multiple device write successful");
                }
                else
                {
                    Console.WriteLine($"Multiple write failed: Error 0x{multiWriteResult:X}");
                }
                
                // Step 8: Working with bit devices (relays)
                // Turn ON internal relay M100
                int relayOnResult = actUtil.SetDevice("M100", 1);
                if (relayOnResult == 0)
                {
                    Console.WriteLine("M100 turned ON");
                }
                
                // Read relay status
                int relayValue;
                int relayReadResult = actUtil.GetDevice("M100", out relayValue);
                if (relayReadResult == 0)
                {
                    Console.WriteLine($"M100 status: {(relayValue == 1 ? "ON" : "OFF")}");
                }
                
                // Step 9: Close connection
                actUtil.Close();
                Console.WriteLine("Connection closed");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred: {ex.Message}");
            }
            */
        }

        /// <summary>
        /// Advanced example showing device monitoring and error recovery
        /// </summary>
        public static void DemonstrateAdvancedUsage()
        {
            /*
            var actUtil = new MITSUBISHI.Component.DotUtlType.ActUtlTypeClass();
            
            try
            {
                // Configure for more robust connection
                actUtil.ActLogicalStationNumber = 1;
                actUtil.ActTimeOut = 5000;  // 5 second timeout
                
                // Open with retry logic
                int retryCount = 3;
                int openResult = -1;
                
                for (int i = 0; i < retryCount; i++)
                {
                    openResult = actUtil.Open();
                    if (openResult == 0)
                    {
                        Console.WriteLine($"Connected on attempt {i + 1}");
                        break;
                    }
                    else
                    {
                        Console.WriteLine($"Connection attempt {i + 1} failed: 0x{openResult:X}");
                        System.Threading.Thread.Sleep(1000); // Wait before retry
                    }
                }
                
                if (openResult != 0)
                {
                    throw new Exception($"Failed to connect after {retryCount} attempts");
                }
                
                // Monitor multiple devices continuously
                var monitorDevices = new List<string> { "D100", "D101", "M100", "M101", "X0", "Y0" };
                var previousValues = new Dictionary<string, int>();
                
                // Initialize previous values
                foreach (string device in monitorDevices)
                {
                    int value;
                    if (actUtil.GetDevice(device, out value) == 0)
                    {
                        previousValues[device] = value;
                    }
                }
                
                // Monitoring loop (would run in separate thread in real application)
                for (int cycle = 0; cycle < 10; cycle++)
                {
                    Console.WriteLine($"Monitoring cycle {cycle + 1}:");
                    
                    foreach (string device in monitorDevices)
                    {
                        int currentValue;
                        int result = actUtil.GetDevice(device, out currentValue);
                        
                        if (result == 0)
                        {
                            if (previousValues.ContainsKey(device) && 
                                previousValues[device] != currentValue)
                            {
                                Console.WriteLine($"  {device}: {previousValues[device]} -> {currentValue}");
                                previousValues[device] = currentValue;
                            }
                        }
                        else
                        {
                            Console.WriteLine($"  {device}: Read error 0x{result:X}");
                        }
                    }
                    
                    System.Threading.Thread.Sleep(1000); // 1 second between cycles
                }
                
                actUtil.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Advanced usage error: {ex.Message}");
                if (actUtil != null)
                {
                    try { actUtil.Close(); } catch { }
                }
            }
            */
        }

        /// <summary>
        /// Example showing integration with the WebInterface architecture
        /// </summary>
        public static void DemonstrateWebInterfaceIntegration()
        {
            /*
            // This shows how the DLL would be used within the WebInterface controller pattern
            
            var mitsubishiController = new MITSUBISHIControllerUtilities();
            
            try
            {
                // Connect to PLC
                mitsubishiController.Connect("PLC1", 1);
                
                // Read devices for web interface
                var deviceList = new List<string> { "D100", "D101", "D102", "M100", "M101" };
                var values = mitsubishiController.ReadDevice("PLC1", deviceList);
                
                // Values would be sent to web interface
                for (int i = 0; i < deviceList.Count; i++)
                {
                    Console.WriteLine($"{deviceList[i]}: {values[i]}");
                }
                
                // Write value from web interface
                mitsubishiController.WriteDevice("PLC1", "D200", 5555);
                
                // Clean up
                mitsubishiController.CloseComm();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"WebInterface integration error: {ex.Message}");
            }
            */
        }

        /// <summary>
        /// Error code reference for troubleshooting
        /// </summary>
        public static void PrintErrorCodeReference()
        {
            Console.WriteLine("MITSUBISHI.Component.DotUtlType.dll Error Code Reference:");
            Console.WriteLine("0x0000 - Success");
            Console.WriteLine("0x1100 - Communication error");
            Console.WriteLine("0x1101 - Connection timeout");
            Console.WriteLine("0x1102 - Invalid station number");
            Console.WriteLine("0x1103 - Password error");
            Console.WriteLine("0x1104 - Device not found");
            Console.WriteLine("0x1105 - Invalid device name");
            Console.WriteLine("0x1106 - Data type mismatch");
            Console.WriteLine("0x1107 - Invalid data range");
            Console.WriteLine("0x1108 - Device memory locked");
            Console.WriteLine("0x1109 - PLC in RUN mode (cannot write to certain devices)");
            Console.WriteLine("0x110A - PLC in STOP mode");
            Console.WriteLine("0x110B - Device write protected");
        }

        /// <summary>
        /// Device type reference for MITSUBISHI PLCs
        /// </summary>
        public static void PrintDeviceTypeReference()
        {
            Console.WriteLine("MITSUBISHI PLC Device Types:");
            Console.WriteLine();
            Console.WriteLine("Data Registers (16-bit):");
            Console.WriteLine("  D0-D32767    - Data registers");
            Console.WriteLine("  W0-W1FFF     - Link registers (hex addressing)");
            Console.WriteLine();
            Console.WriteLine("Bit Devices (ON/OFF):");
            Console.WriteLine("  M0-M32767    - Internal relays");
            Console.WriteLine("  X0-X1777     - Input contacts (octal addressing)");
            Console.WriteLine("  Y0-Y1777     - Output contacts (octal addressing)");
            Console.WriteLine("  B0-B1FFF     - Link relays (hex addressing)");
            Console.WriteLine("  L0-L32767    - Latch relays");
            Console.WriteLine("  F0-F32767    - Annunciator flags");
            Console.WriteLine("  V0-V32767    - Edge relays");
            Console.WriteLine();
            Console.WriteLine("Timers and Counters:");
            Console.WriteLine("  T0-T1023     - Timers");
            Console.WriteLine("  ST0-ST1023   - Retentive timers");
            Console.WriteLine("  C0-C1023     - Counters");
            Console.WriteLine();
            Console.WriteLine("Special Devices:");
            Console.WriteLine("  SD0-SD32767  - Special data registers");
            Console.WriteLine("  SM0-SM32767  - Special internal relays");
        }
    }
}