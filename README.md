# Uni-Project-Music-player
I Made a Music player for a uni project I made this in Visual studio .Net framework in C# (winforms)
# 🎵 AI-Powered Music Player

A modern, feature-rich Windows desktop music player built with C# and Windows Forms, featuring intelligent song recommendations powered by Last.fm API.



## ✨ Features

### 🎧 Core Functionality
- **Multi-format Support**: Play MP3, WAV, WMA, M4A, and AAC files
- **Playlist Management**: Add multiple songs, organize your music library
- **Intuitive Controls**: Play, pause, skip, seek, and volume control
- **Progress Tracking**: Real-time playback progress with seekable timeline
- **Modern Dark UI**: Sleek Spotify-inspired interface with green accents

### 🤖 Smart Recommendations
- **AI-Powered Suggestions**: Get personalized song recommendations using Last.fm API
- **Playlist Analysis**: Recommends songs from your library based on listening patterns
- **Similar Track Discovery**: Find new music similar to your current selection
- **Dual Recommendation System**: Shows matches from both your playlist and Last.fm's database

### 🎨 User Interface
- **Clean Design**: Professional dark theme with intuitive layout
- **Visual Feedback**: Dynamic progress bars, time displays, and volume indicators
- **Responsive Controls**: Smooth animations and immediate feedback
- **Album Art Placeholder**: Music note icon with customizable design

<img width="983" height="561" alt="image" src="https://github.com/user-attachments/assets/76a72b86-c024-4ed7-a74c-998f7f439aed" />


---

## 🚀 Getting Started

### Prerequisites

- **Operating System**: Windows 7 or later
- **Framework**: .NET Framework 4.7.2 or higher
- **IDE**: Visual Studio 2019 or later (Community Edition works!)

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/yourusername/music-player.git
   cd music-player
   ```

2. **Open in Visual Studio**
   - Double-click `MusicPlayer.sln`
   - Wait for Visual Studio to restore packages

3. **Add Windows Media Player COM Reference**
   - Right-click on `References` in Solution Explorer
   - Select `Add Reference...`
   - Go to `COM` tab
   - Check `Windows Media Player`
   - Click `OK`

4. **Install NuGet Package**
   - Right-click project → `Manage NuGet Packages`
   - Search for `Newtonsoft.Json`
   - Click `Install`

5. **Get Last.fm API Key** (Optional but recommended)
   - Fill in application details:
     - **Application Name**: My Music Player
     - **Application Description**: Personal music player with recommendations
   - Copy your API key
   - Open `MusicPlayerForm.cs`
   - Replace `YOUR_LASTFM_API_KEY_HERE` with your actual key (line ~48)
   
   ```csharp
   private const string LASTFM_API_KEY = "your_api_key_here";
   ```

6. **Build and Run**
   - Press `F5` or click `Start`
   - Enjoy your music! 🎉

---

## 📖 How to Use

### Adding Songs to Your Playlist

1. Click the **"➕ Add Songs"** button
2. Select one or multiple music files (Hold `Ctrl` to select multiple)
3. Songs appear in the playlist on the right

**Pro Tip**: Name your files as `"Artist - Song Title.mp3"` for best results!
- Example: `The Weeknd - Blinding Lights.mp3`


### Playing Music

- **Double-click** any song in the playlist to play it
- Use **Play/Pause** button (▶/⏸) to control playback
- **Previous** button (⏪): Restart song or go to previous track
- **Next** button (⏩): Skip to next song

### Using Volume Control

- Drag the **volume slider** to adjust volume (0-100%)
- Current volume percentage is displayed

### Seeking in Songs

- **Click or drag** the progress bar to jump to any part of the song
- Current time and total duration displayed below progress bar

### Getting AI Recommendations

1. Play any song from your playlist
2. Click **"🎵 AI: Suggest Next Song"** button
3. Get personalized recommendations based on:
   - Current song's genre and style
   - Artist similarity
   - Songs in your playlist
   - Last.fm's massive music database


---

## 🏗️ Architecture & Technologies

### Core Technologies

| Technology | Purpose |
|------------|---------|
| **C# / .NET Framework** | Primary programming language and runtime |
| **Windows Forms** | Desktop UI framework |
| **Windows Media Player COM** | Audio playback engine |
| **Newtonsoft.Json** | JSON parsing for API responses |
| **Last.fm API** | Music recommendation service |

### Project Structure

```
MusicPlayer/
├── MusicPlayerForm.cs          # Main form logic
├── MusicPlayerForm.Designer.cs # Auto-generated UI code
├── Program.cs                  # Application entry point
├── Properties/                 # Assembly info and resources
├── Images/                     # UI icons (optional)
└── README.md                   # This file
```

### Key Components

#### 1. **Media Playback**
- Windows Media Player COM handles all audio operations
- Supports multiple formats through Windows codecs
- Real-time position tracking via Timer (updates every 100ms)

#### 2. **Playlist Management**
- Custom `SongInfo` class stores song metadata
- `List<SongInfo>` dynamically manages playlist
- Automatic artist/title parsing from filenames

#### 3. **API Integration**
- Asynchronous HTTP requests to Last.fm
- JSON response parsing with Newtonsoft.Json
- Intelligent matching between API results and local playlist

#### 4. **UI/UX**
- Event-driven architecture for user interactions
- Safe null-checking prevents crashes
- Try-catch blocks ensure stability

---

## 🎨 Customization

### Changing Colors

Edit these RGB values in `InitializeComponent()`:

```csharp
// Primary accent color (Spotify green)
Color.FromArgb(29, 185, 84)

// Background color
Color.FromArgb(18, 18, 18)

// Panel backgrounds
Color.FromArgb(40, 40, 40)

// Text color
Color.FromArgb(180, 180, 180)

// Purple accent (AI button)
Color.FromArgb(138, 43, 226)
```

### Adding Custom Button Icons

1. Create an `Images` folder in your project directory
2. Add PNG images:
   - `play.png`
   - `pause.png`
   - `previous.png`
   - `next.png`
3. Set "Copy to Output Directory" to "Copy if newer"
4. Uncomment image-loading code in `LoadButtonImage()` method

### Changing Fonts

```csharp
new Font("Segoe UI", 24F, FontStyle.Bold)
//       ^Font Name  ^Size  ^Style
```

Replace `"Segoe UI"` with any installed font.

---

## 🔧 Troubleshooting

### Build Errors

**Problem**: `WMPLib` not found
- **Solution**: Add Windows Media Player COM reference (see Installation step 3)

**Problem**: `JsonConvert` not found
- **Solution**: Install Newtonsoft.Json NuGet package (see Installation step 4)

**Problem**: Multiple entry points
- **Solution**: Remove `Main()` method from `MusicPlayerForm.cs` if present

### Runtime Errors

**Problem**: No sound plays
- **Solution**: Check Windows volume, try different audio file formats

**Problem**: "Object reference not set" errors
- **Solution**: Load a song before clicking play/pause

**Problem**: API recommendations not working
- **Solution**: Verify Last.fm API key is correctly set

### Performance Issues

**Problem**: UI freezes when loading songs
- **Solution**: This is normal for large libraries; consider loading in background

**Problem**: Progress bar stutters
- **Solution**: Reduce timer interval (currently 100ms)

---

## 🤝 Contributing

Contributions are welcome! Here's how you can help:

1. **Fork** the repository
2. **Create** a feature branch (`git checkout -b feature/AmazingFeature`)
3. **Commit** your changes (`git commit -m 'Add some AmazingFeature'`)
4. **Push** to the branch (`git push origin feature/AmazingFeature`)
5. **Open** a Pull Request

### Ideas for Contributions

- [ ] Add album art display from ID3 tags
- [ ] Implement shuffle and repeat modes
- [ ] Add equalizer controls
- [ ] Support for playlists (save/load)
- [ ] Lyrics display integration
- [ ] Keyboard shortcuts
- [ ] System tray integration
- [ ] Music library scanning
- [ ] Tag editor
- [ ] Visualization effects

---

## 📜 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

```
MIT License

Copyright (c) 2024 [Your Name]

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.
```

---

## 🙏 Acknowledgments

- **Last.fm** for their excellent music API
- **Microsoft** for Windows Media Player and .NET Framework
- **Newtonsoft** for Json.NET library
- **Spotify** for design inspiration
- **All contributors** who help improve this project



---

## 🗺️ Roadmap

### Version 1.0 ✅
- [x] Basic playback controls
- [x] Playlist management
- [x] Last.fm integration
- [x] Modern UI design

### Version 2.0 (Planned)
- [ ] Album art from metadata
- [ ] Advanced playlist features
- [ ] Theme customization
- [ ] Keyboard shortcuts
- [ ] Mini player mode

### Version 3.0 (Future)
- [ ] Online streaming integration
- [ ] Social features
- [ ] Cloud sync
- [ ] Mobile companion app

---

**Made with ❤️ and lots of ☕ by Azfar Shafiq**

*If you find this project helpful, please consider giving it a ⭐!*
