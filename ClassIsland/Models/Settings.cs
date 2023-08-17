﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Converters;
using ClassIsland.Enums;
using CommunityToolkit.Mvvm.ComponentModel;
using IWshRuntimeLibrary;
using MaterialDesignColors;
using Newtonsoft.Json;
using File = System.IO.File;

namespace ClassIsland.Models;

public class Settings : ObservableRecipient
{
    private int _theme = 0;
    private Color _primaryColor = Colors.DeepSkyBlue;
    private Color _secondaryColor = Colors.Aquamarine;
    private DateTime _singleWeekStartTime = DateTime.Now;
    private int _classPrepareNotifySeconds = 60;
    private bool _showDate = true;
    private bool _hideOnClass = false;
    private bool _isClassChangingNotificationEnabled = true;
    private bool _isClassPrepareNotificationEnabled = true;
    private int _windowDockingLocation = 1;
    private int _windowDockingOffsetX = 0;
    private int _windowDockingOffsetY = 0;
    private int _windowDockingMonitorIndex = 0;
    private int _windowLayer = 0;
    private bool _isMouseClickingEnabled = false;
    private bool _hideOnFullscreen = true;
    private bool _isClassOffNotificationEnabled = true;
    private ObservableCollection<string> _excludedFullscreenWindow = new()
    {
        "explorer"
    };

    private bool _hideOnMaxWindow = false;
    private double _opacity = 0.5;
    private bool _isDebugEnabled = false;
    private string _selectedProfile = "Default.json";
    private bool _isWelcomeWindowShowed = false;
    private bool _isReportingEnabled = true;
    private Dictionary<string, string> _releaseChannels = new()
    {
    };

    private string _selectedChannel = "https://install.appcenter.ms/api/v0.1/apps/hellowrc/classisland/distribution_groups/public";
    private DateTime _lastCheckUpdateTime = DateTime.MinValue;
    private AppCenterReleaseInfo _lastCheckUpdateInfoCache = new();
    private UpdateStatus _lastUpdateStatus = UpdateStatus.UpToDate;
    private int _updateMode = 3;
    private bool _autoInstallUpdateNextStartup = true;
    private bool _isDebugOptionsEnabled = false;
    private Color _selectedPlatte = Colors.DodgerBlue;
    private int _selectedPlatteIndex = 0;
    private ObservableCollection<Color> _wallpaperColorPlatte = new(Enumerable.Repeat(Colors.DodgerBlue, 5));
    private int _colorSource = 1;
    private string _wallpaperClassName = "";
    private double _targetLightValue = 0.6;
    private double _scale = 1.0;
    private bool _isMainWindowDebugEnabled = false;
    private ObservableDictionary<string, bool> _notificationProvidersEnableStates = new();
    private ObservableCollection<string> _notificationProvidersPriority = new();
    private ObservableDictionary<string, object?> _notificationProvidersSettings = new();

    public void NotifyPropertyChanged(string propertyName)
    {
        OnPropertyChanged(propertyName);
    }
 
    public string SelectedProfile
    {
        get => _selectedProfile;
        set
        {
            if (value == _selectedProfile) return;
            _selectedProfile = value;
            OnPropertyChanged();
        }
    }

    public bool IsWelcomeWindowShowed
    {
        get => _isWelcomeWindowShowed;
        set
        {
            if (value == _isWelcomeWindowShowed) return;
            _isWelcomeWindowShowed = value;
            OnPropertyChanged();
        }
    }

    #region Gerneral

    public DateTime SingleWeekStartTime
    {
        get => _singleWeekStartTime;
        set
        {
            if (value.Equals(_singleWeekStartTime)) return;
            _singleWeekStartTime = value;
            OnPropertyChanged();
        }
    }

    public int ClassPrepareNotifySeconds
    {
        get => _classPrepareNotifySeconds;
        set
        {
            if (value == _classPrepareNotifySeconds) return;
            _classPrepareNotifySeconds = value;
            OnPropertyChanged();
        }
    }

    public bool IsClassPrepareNotificationEnabled
    {
        get => _isClassPrepareNotificationEnabled;
        set
        {
            if (value == _isClassPrepareNotificationEnabled) return;
            _isClassPrepareNotificationEnabled = value;
            OnPropertyChanged();
        }
    }

    public bool ShowDate
    {
        get => _showDate;
        set
        {
            if (value == _showDate) return;
            _showDate = value;
            OnPropertyChanged();
        }
    }

    public bool HideOnClass
    {
        get => _hideOnClass;
        set
        {
            if (value == _hideOnClass) return;
            _hideOnClass = value;
            OnPropertyChanged();
        }
    }

    public bool IsClassChangingNotificationEnabled
    {
        get => _isClassChangingNotificationEnabled;
        set
        {
            if (value == _isClassChangingNotificationEnabled) return;
            _isClassChangingNotificationEnabled = value;
            OnPropertyChanged();
        }
    }

    public bool IsClassOffNotificationEnabled
    {
        get => _isClassOffNotificationEnabled;
        set
        {
            if (value == _isClassOffNotificationEnabled) return;
            _isClassOffNotificationEnabled = value;
            OnPropertyChanged();
        }
    }

    public bool HideOnFullscreen
    {
        get => _hideOnFullscreen;
        set
        {
            if (value == _hideOnFullscreen) return;
            _hideOnFullscreen = value;
            OnPropertyChanged();
        }
    }

    public ObservableCollection<string> ExcludedFullscreenWindow
    {
        get => _excludedFullscreenWindow;
        set
        {
            if (Equals(value, _excludedFullscreenWindow)) return;
            _excludedFullscreenWindow = value;
            OnPropertyChanged();
        }
    }

    public bool HideOnMaxWindow
    {
        get => _hideOnMaxWindow;
        set
        {
            if (value == _hideOnMaxWindow) return;
            _hideOnMaxWindow = value;
            OnPropertyChanged();
        }
    }

    public bool IsAutoStartEnabled
    {
        get => File.Exists(
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Startup), "ClassIsland.lnk"));
        set
        {
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Startup), "ClassIsland.lnk");
            try
            {
                if (value)
                {
                    var shell = new WshShell();
                    var shortcut = (IWshShortcut)shell.CreateShortcut(path);//创建快捷方式对象
                    shortcut.TargetPath = Environment.ProcessPath;
                    shortcut.WorkingDirectory = Environment.CurrentDirectory;
                    shortcut.WindowStyle = 1;
                    shortcut.Save();
                }
                else
                {
                    File.Delete(path);
                }
                OnPropertyChanged();
            }
            catch
            {
                // ignored
            }
        }
    }

    public bool IsReportingEnabled
    {
        get => _isReportingEnabled;
        set
        {
            if (value == _isReportingEnabled) return;
            _isReportingEnabled = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region Appearence

    public int Theme
    {
        get => _theme;
        set
        {
            if (value == _theme) return;
            _theme = value;
            OnPropertyChanged();
        }
    }

    public Color PrimaryColor
    {
        get => _primaryColor;
        set
        {
            if (value.Equals(_primaryColor)) return;
            _primaryColor = value;
            OnPropertyChanged();
        }
    }

    public Color SecondaryColor
    {
        get => _secondaryColor;
        set
        {
            if (value.Equals(_secondaryColor)) return;
            _secondaryColor = value;
            OnPropertyChanged();
        }
    }

    public int ColorSource
    {
        get => _colorSource;
        set
        {
            if (value == _colorSource) return;
            _colorSource = value;
            OnPropertyChanged();
        }
    }

    public ObservableCollection<Color> WallpaperColorPlatte
    {
        get => _wallpaperColorPlatte;
        set
        {
            if (Equals(value, _wallpaperColorPlatte)) return;
            _wallpaperColorPlatte = value;
            OnPropertyChanged();
        }
    }

    [JsonIgnore]
    public Color SelectedPlatte => WallpaperColorPlatte.Count < Math.Max(SelectedPlatteIndex, 0) + 1 
        ? Colors.DodgerBlue 
        : WallpaperColorPlatte[SelectedPlatteIndex];

    public int SelectedPlatteIndex
    {
        get => _selectedPlatteIndex;
        set
        {
            if (value == _selectedPlatteIndex) return;
            _selectedPlatteIndex = value;
            OnPropertyChanged();
        }
    }

    public string WallpaperClassName
    {
        get => _wallpaperClassName;
        set
        {
            if (value == _wallpaperClassName) return;
            _wallpaperClassName = value;
            OnPropertyChanged();
        }
    }

    public double TargetLightValue
    {
        get => _targetLightValue;
        set
        {
            if (value.Equals(_targetLightValue)) return;
            _targetLightValue = value;
            OnPropertyChanged();
        }
    }

    public double Opacity
    {
        get => _opacity;
        set
        {
            if (value.Equals(_opacity)) return;
            _opacity = value;
            OnPropertyChanged();
        }
    }

    public double Scale
    {
        get => _scale;
        set
        {
            if (value.Equals(_scale)) return;
            _scale = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region Notifications

    public ObservableDictionary<string, bool> NotificationProvidersEnableStates
    {
        get => _notificationProvidersEnableStates;
        set
        {
            if (Equals(value, _notificationProvidersEnableStates)) return;
            _notificationProvidersEnableStates = value;
            OnPropertyChanged();
        }
    }

    public ObservableCollection<string> NotificationProvidersPriority
    {
        get => _notificationProvidersPriority;
        set
        {
            if (Equals(value, _notificationProvidersPriority)) return;
            _notificationProvidersPriority = value;
            OnPropertyChanged();
        }
    }

    public ObservableDictionary<string, object?> NotificationProvidersSettings
    {
        get => _notificationProvidersSettings;
        set
        {
            if (Equals(value, _notificationProvidersSettings)) return;
            _notificationProvidersSettings = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region AppUpgrades

    /// <summary>
    /// 更新模式
    /// </summary>
    /// 
    public int UpdateMode
    {
        get => _updateMode;
        set
        {
            if (value == _updateMode) return;
            _updateMode = value;
            OnPropertyChanged();
        }
    }

    public Dictionary<string, string> ReleaseChannels
    {
        get => _releaseChannels;
        set
        {
            if (Equals(value, _releaseChannels)) return;
            _releaseChannels = value;
            OnPropertyChanged();
        }
    }

    public string SelectedChannel
    {
        get => _selectedChannel;
        set
        {
            if (value == _selectedChannel) return;
            _selectedChannel = value;
            OnPropertyChanged();
        }
    }

    public DateTime LastCheckUpdateTime
    {
        get => _lastCheckUpdateTime;
        set
        {
            if (value.Equals(_lastCheckUpdateTime)) return;
            _lastCheckUpdateTime = value;
            OnPropertyChanged();
        }
    }

    public AppCenterReleaseInfo LastCheckUpdateInfoCache
    {
        get => _lastCheckUpdateInfoCache;
        set
        {
            if (Equals(value, _lastCheckUpdateInfoCache)) return;
            _lastCheckUpdateInfoCache = value;
            OnPropertyChanged();
        }
    }

    public UpdateStatus LastUpdateStatus
    {
        get => _lastUpdateStatus;
        set
        {
            if (value == _lastUpdateStatus) return;
            _lastUpdateStatus = value;
            OnPropertyChanged();
        }
    }

    public bool AutoInstallUpdateNextStartup
    {
        get => _autoInstallUpdateNextStartup;
        set
        {
            if (value == _autoInstallUpdateNextStartup) return;
            _autoInstallUpdateNextStartup = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region Window

    /// <summary>
    /// 窗口停靠位置
    /// 
    /// </summary>
    /// <value>
    /// <code>
    /// #############
    /// # 0   1   2 #
    /// #           #
    /// # 3   4   5 #
    /// #############
    /// </code>
    /// </value>
    public int WindowDockingLocation
    {
        get => _windowDockingLocation;
        set
        {
            if (value == _windowDockingLocation) return;
            _windowDockingLocation = value;
            OnPropertyChanged();
        }
    }

    public int WindowDockingOffsetX
    {
        get => _windowDockingOffsetX;
        set
        {
            if (value == _windowDockingOffsetX) return;
            _windowDockingOffsetX = value;
            OnPropertyChanged();
        }
    }

    public int WindowDockingOffsetY
    {
        get => _windowDockingOffsetY;
        set
        {
            if (value == _windowDockingOffsetY) return;
            _windowDockingOffsetY = value;
            OnPropertyChanged();
        }
    }

    public int WindowDockingMonitorIndex
    {
        get => _windowDockingMonitorIndex;
        set
        {
            if (value == _windowDockingMonitorIndex) return;
            _windowDockingMonitorIndex = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// 窗口层级
    /// </summary>
    /// <value>
    /// 0 - 置底<br/>
    /// 1 - 置顶
    /// </value>
    public int WindowLayer
    {
        get => _windowLayer;
        set
        {
            if (value == _windowLayer) return;
            _windowLayer = value;
            OnPropertyChanged();
        }
    }

    public bool IsMouseClickingEnabled
    {
        get => _isMouseClickingEnabled;
        set
        {
            if (value == _isMouseClickingEnabled) return;
            _isMouseClickingEnabled = value;
            OnPropertyChanged();
        }
    }

    #endregion

    public bool IsDebugEnabled
    {
        get => _isDebugEnabled;
        set
        {
            if (value == _isDebugEnabled) return;
            _isDebugEnabled = value;
            OnPropertyChanged();
        }
    }

    public bool IsDebugOptionsEnabled
    {
        get => _isDebugOptionsEnabled;
        set
        {
            if (value == _isDebugOptionsEnabled) return;
            _isDebugOptionsEnabled = value;
            OnPropertyChanged();
        }
    }

    public bool IsMainWindowDebugEnabled
    {
        get => _isMainWindowDebugEnabled;
        set
        {
            if (value == _isMainWindowDebugEnabled) return;
            _isMainWindowDebugEnabled = value;
            OnPropertyChanged();
        }
    }
}