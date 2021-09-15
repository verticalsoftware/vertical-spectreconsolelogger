# Extending The Provider

## Overview

Extending the logging provider cam be accomplished primarily by creating and registering your own [ICustomFormatter](https://docs.microsoft.com/en-us/dotnet/api/system.icustomformatter?view=net-5.0) and customizing output templates, but if you need to add functionality to the output template you do so by writing your own custom renderer.

### Creating a custom renderer

Imagine we're writing an application that on events with a specific event ID, we need to display the application's memory usage. The easiest way to do this would be to create a custom renderer. In the example below, we'll create a renderer that even supports custom formatting. 