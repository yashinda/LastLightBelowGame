# SaveData_Pro - Unity Save Data Management System

A comprehensive, secure, and high-performance save data management system for Unity games.

## Features

- **Multiple Serialization Methods**: JSON, Optimized JSON, and Safe Binary serialization
- **AES-256 Encryption**: Secure data encryption with configurable key sizes
- **Data Integrity Verification**: SHA256 hash verification to detect data tampering
- **Auto Save System**: Automatic periodic saving with configurable intervals
- **Modular Architecture**: Dependency injection pattern for easy testing and extensibility
- **Performance Optimized**: Memory-efficient operations with minimal garbage collection
- **Cross-Platform**: Works on all Unity-supported platforms with platform-specific optimizations
- **Easy Integration**: Simple API with intuitive method names

## Quick Start

### Basic Usage

```csharp
// Save data
DataManager.Save("player_name", "John Doe");
DataManager.Save("player_level", 25);
DataManager.Save("player_position", new Vector3(10, 0, 5));

// Load data
string playerName = DataManager.Load("player_name", "DefaultName");
int playerLevel = DataManager.Load("player_level", 1);
Vector3 playerPosition = DataManager.Load("player_position", Vector3.zero);

// Check if data exists
if (DataManager.Exists("player_name"))
{
    Debug.Log("Player data found!");
}

// Delete data
DataManager.Delete("old_save_data");
```

### Working with Complex Data

```csharp
// Define your data class
[System.Serializable]
public class PlayerData
{
    public string playerName;
    public int level;
    public List<string> inventory;
    public Dictionary<string, int> stats;
}

// Save complex data
var playerData = new PlayerData
{
    playerName = "Hero",
    level = 10,
    inventory = new List<string> { "Sword", "Shield", "Potion" },
    stats = new Dictionary<string, int> { {"Health", 100}, {"Mana", 50} }
};

DataManager.Save("player_data", playerData);

// Load complex data
var loadedData = DataManager.Load<PlayerData>("player_data");
```

### Game Settings Management

```csharp
// Save game settings
var settings = new GameSettings
{
    masterVolume = 0.8f,
    fullscreen = true,
    language = "en"
};

DataManager.SaveGameSettings(settings);

// Load game settings
var loadedSettings = DataManager.LoadGameSettings<GameSettings>();
```

### Player Progress with Multiple Save Slots

```csharp
// Save to different slots
DataManager.SavePlayerProgress(playerData, 0); // Slot 1
DataManager.SavePlayerProgress(playerData, 1); // Slot 2
DataManager.SavePlayerProgress(playerData, 2); // Slot 3

// Load from specific slot
var slotData = DataManager.LoadPlayerProgress<PlayerData>(1);
```

### Auto Save System

SaveData_Pro includes a comprehensive auto save system that automatically saves registered data at specified intervals:

```csharp
// Register objects for auto save
var playerData = new PlayerData { playerName = "Hero", level = 10 };
var gameSettings = new GameSettings { masterVolume = 0.8f };

AutoSaveManager.Instance.Register(playerData, "player_data");
AutoSaveManager.Instance.Register(gameSettings, "game_settings");

// The system will automatically save these objects based on the configured interval
```

#### Auto Save Configuration

Configure auto save settings in `SaveDataProConfig`:

```csharp
var config = SaveDataProConfig.Instance;
config.enableAutoSave = true;          // Enable auto save
config.autoSaveInterval = 300f;        // Save every 5 minutes (300 seconds)
```

#### Manual Auto Save Control

```csharp
// Register additional data for auto save
AutoSaveManager.Instance.Register(myInventory, "inventory_data");

// Unregister when no longer needed
AutoSaveManager.Instance.Unregister("inventory_data");

// Auto save will automatically start when objects are registered
// and stop when no objects are registered
```

#### Platform-Specific Auto Save

Auto save intervals are automatically optimized for different platforms:

- **Mobile Platforms**: 3-minute intervals (frequent saves for app backgrounding)
- **Desktop Platforms**: 5-minute intervals (balanced performance)
- **WebGL**: 2-minute intervals (browser stability considerations)

```csharp
// Use platform-optimized auto save settings
DataManager.InitializePlatformOptimized();
// This automatically configures appropriate auto save intervals for the current platform
```

#### Auto Save Best Practices

1. **Register Important Data**: Only register data that needs frequent saving
2. **Use Meaningful Keys**: Use descriptive keys for easy identification
3. **Update References**: Re-register objects when they change significantly
4. **Monitor Performance**: Adjust intervals based on your game's needs

```csharp
// Example: Auto save for a complex game state
public class GameStateManager : MonoBehaviour
{
    public PlayerData playerData;
    public WorldState worldState;
    
    void Start()
    {
        // Register critical game data for auto save
        AutoSaveManager.Instance.Register(playerData, "player_data");
        AutoSaveManager.Instance.Register(worldState, "world_state");
        
        Debug.Log("Auto save enabled for critical game data");
    }
    
    void OnDestroy()
    {
        // Clean up auto save registrations
        AutoSaveManager.Instance.Unregister("player_data");
        AutoSaveManager.Instance.Unregister("world_state");
    }
}
```

## Configuration

The system uses `SaveDataProConfig` for global configuration. You can customize various aspects:

### Encryption Settings
- **AES Key Size**: 128-bit, 192-bit, or 256-bit (default: 256-bit)
- **Default Encryption Key**: Customize your encryption key
- **Enable/Disable Encryption**: Toggle encryption for performance

### Data Integrity
- **Secret Salt**: Custom salt for hash verification
- **Integrity Check**: Enable/disable data tampering detection

### Serialization Options
- **JSON**: Human-readable, cross-platform compatible
- **Optimized JSON**: Performance-optimized with reduced memory allocation
- **Safe Binary**: Compact size, faster processing

### File System
- **Custom Save Directory**: Specify custom save location
- **File Prefix/Extension**: Customize file naming

### Performance
- **Cache Settings**: Configure memory cache size
- **Auto-Save**: Enable periodic automatic saving
- **Verbose Logging**: Detailed debugging information

## Advanced Usage

### Custom Initialization

```csharp
// Initialize with custom settings
var serializer = new JsonSerializerOption();
var encryption = new AesEncryptionOption("MySecretKey2025");
var provider = new FileDataProvider(serializer, encryption, true);

DataManager.Initialize(provider);
```

### Performance Optimization

```csharp
// For performance-critical scenarios, disable encryption and integrity check
var optimizedSerializer = new OptimizedJsonSerializerOption();
var provider = new FileDataProvider(optimizedSerializer, null, false);
DataManager.Initialize(provider);
```

### Working with Multiple Data Providers

```csharp
// You can create different providers for different types of data
var secureProvider = new FileDataProvider(
    new JsonSerializerOption(), 
    new AesEncryptionOption("SecureKey"), 
    true
);

var fastProvider = new FileDataProvider(
    new OptimizedJsonSerializerOption(), 
    null, 
    false
);
```

## Supported Data Types

SaveData_Pro supports a wide range of data types:

### Primitive Types
- `string`, `int`, `float`, `bool`, `double`, `long`

### Unity Types
- `Vector2`, `Vector3`, `Vector4`
- `Quaternion`
- `Color`, `Color32`

### Collections
- `List<T>`
- `Dictionary<TKey, TValue>`
- Arrays

### Custom Classes
- Any class marked with `[System.Serializable]`
- Classes with proper constructors for JSON serialization

## Security Features

### Encryption
- **AES-256 Encryption**: Industry-standard encryption
- **Random IV**: Each save operation uses a unique initialization vector
- **Key Derivation**: Secure key generation from string passwords

### Data Integrity
- **SHA256 Hashing**: Detects data corruption or tampering
- **Salted Hashes**: Additional security layer against rainbow table attacks
- **Automatic Verification**: Data integrity checked on every load operation

## Error Handling

The system includes comprehensive error handling:

```csharp
try
{
    DataManager.Save("test_data", someData);
}
catch (System.Exception e)
{
    Debug.LogError($"Failed to save data: {e.Message}");
}

// Load with fallback
var data = DataManager.Load("test_data", defaultValue);
if (data == defaultValue)
{
    Debug.Log("No save data found, using default values");
}
```

## Best Practices

### Performance
1. **Use Optimized JSON** for frequently accessed data
2. **Disable encryption** for non-sensitive data to improve performance
3. **Batch operations** when possible to reduce I/O overhead
4. **Use appropriate data types** - avoid complex nested structures when not needed

### Security
1. **Use unique encryption keys** for different games/projects
2. **Enable integrity checking** for important save data
3. **Validate loaded data** before using it in your game logic
4. **Handle encryption failures** gracefully

### Data Organization
1. **Use meaningful key names** for easy debugging
2. **Group related data** into classes/structs
3. **Use save slots** for multiple player profiles
4. **Separate settings from game progress** data

## Demo Scripts

The package includes several demo scripts to help you get started:

- **SaveDataDemo**: Comprehensive demonstration of all features
- **PerformantSaveDataDemo**: Performance-optimized examples  
- **SerializationBenchmark**: Performance comparison between serialization methods
- **SaveDataDemoManager**: Manager for switching between demo modes
- **CrossPlatformDemo**: Cross-platform compatibility testing and platform-specific optimizations

## Troubleshooting

### Common Issues

1. **Data Not Saving**
   - Check if the save directory has write permissions
   - Verify that the data class is properly serializable
   - Check for exceptions in the console

2. **Decryption Failures**
   - Ensure you're using the same encryption key
   - Verify that the data wasn't corrupted during transfer
   - Check if the file was modified externally

3. **Performance Issues**
   - Disable encryption for non-sensitive data
   - Use OptimizedJsonSerializerOption for better performance
   - Reduce the frequency of save operations

4. **Serialization Errors**
   - Ensure all classes have `[System.Serializable]` attribute
   - Check for circular references in your data structures
   - Verify that all fields are public or have proper getters/setters

### Debug Mode

Enable verbose logging in `SaveDataProConfig` to get detailed information about save/load operations:

```csharp
var config = SaveDataProConfig.Instance;
config.enableVerboseLogging = true;
```

## Platform Considerations

SaveData_Pro provides comprehensive cross-platform support with platform-specific optimizations:

### Automatic Platform Detection
The system automatically detects the current platform and applies appropriate optimizations:

```csharp
// Initialize with platform-specific optimizations
DataManager.InitializePlatformOptimized();
```

### Platform-Specific Features

#### Mobile Platforms (Android/iOS)
- **Optimized for App Lifecycle**: Enhanced auto-save for backgrounding scenarios
- **Storage Efficient**: Reduced cache size and backup count for limited storage
- **Security Focused**: Encryption enabled by default for sensitive user data
- **Battery Conscious**: Optimized serialization to reduce CPU usage
- **Auto-Save**: 3-minute intervals to handle unexpected app termination

#### Desktop Platforms (Windows/macOS/Linux)
- **Full Feature Set**: All encryption and integrity features enabled
- **Larger Cache**: Up to 100MB cache for better performance
- **Custom Directories**: Support for user-specified save locations
- **Verbose Logging**: Detailed debugging information available
- **Flexible Storage**: Support for multiple backup files

#### Web Platform (WebGL)
- **Performance Optimized**: Encryption disabled by default for better performance
- **Compact Storage**: Shorter file paths and extensions to save space
- **Memory Efficient**: Optimized JSON serialization with minimal allocations
- **IndexedDB Compatible**: Works within browser storage limitations
- **Reduced Logging**: Minimal console output to improve performance

### Platform-Specific Configuration

```csharp
// Get platform-optimized configuration
var config = SaveDataProConfig.CreatePlatformOptimized();

// Platform-specific settings are automatically applied:
// - WebGL: Optimized JSON, no encryption, 10MB cache
// - Mobile: Standard settings with frequent auto-save
// - Desktop: Full features with large cache
```

### Cross-Platform Data Compatibility

SaveData_Pro ensures data saved on one platform can be loaded on another:

- **JSON Serialization**: Human-readable and cross-platform compatible
- **Unicode Support**: Full UTF-8 support for international characters
- **Consistent Paths**: Automatic path normalization across operating systems
- **Endian Safety**: All data types handle byte order differences automatically

### Platform Testing

Use the included `CrossPlatformDemo` script to test platform-specific features:

```csharp
// Test cross-platform compatibility
var demo = gameObject.AddComponent<CrossPlatformDemo>();
demo.usePlatformOptimization = true;
```

## API Reference

### DataManager Static Methods

| Method | Description |
|--------|-------------|
| `Save<T>(string key, T data)` | Save data with specified key |
| `Load<T>(string key, T defaultValue)` | Load data or return default value |
| `Exists(string key)` | Check if data exists |
| `Delete(string key)` | Delete data by key |
| `GetAllKeys()` | Get all saved keys (FileDataProvider only) |
| `SaveGameSettings(object settings)` | Save game settings |
| `LoadGameSettings<T>(T defaultSettings)` | Load game settings |
| `SavePlayerProgress(object data, int slot)` | Save player progress to slot |
| `LoadPlayerProgress<T>(int slot, T defaultData)` | Load player progress from slot |
| `InitializeDefault()` | Initialize with default configuration |
| `InitializePlatformOptimized()` | **NEW**: Initialize with platform-specific optimizations |
| `Initialize(IDataProvider provider)` | Initialize with custom data provider |

### SaveDataProConfig Methods

| Method | Description |
|--------|-------------|
| `CreatePlatformOptimized()` | **NEW**: Create platform-specific configuration |
| `GetSaveDirectory()` | Get platform-optimized save directory path |
| `LoadConfig()` | Load configuration from file |
| `SaveConfig()` | Save configuration to file |
| `ResetToDefault()` | Reset to default configuration |

### Configuration Properties

Refer to `SaveDataProConfig` class for all available configuration options including encryption settings, serialization options, and performance tuning parameters.

## License

This project is provided as-is for educational and commercial use. Please refer to the license file for detailed terms and conditions.

## Support

For questions, bug reports, or feature requests, please check the documentation or contact the development team.
