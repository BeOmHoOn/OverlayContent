># OverlayContent
>Overlay Text/Content FrameworkElement for WPF<br/>

# Exmaples
>### Example Image
![exmaple](./OverlayContentForWPF.PNG)

>### Example Xaml
<pre>
<code>
<Grid>
    <c:OverlayedText
        Panel.ZIndex="1"
        Fill="{StaticResource ThemeDefault.Color.LightBlue}"
        FontFamily="Lane"
        FontSize="50"
        Opacity=".8" />
    <Path
        Width="150"
        HorizontalAlignment="Center"
        VerticalAlignment="Center"
        Data="{StaticResource Path.Creeper}"
        Fill="{StaticResource ThemeDefault.Black}"
        Stretch="Uniform" />
    <TextBlock
        HorizontalAlignment="Center"
        VerticalAlignment="Bottom"
        Panel.ZIndex="0"
        FontSize="70"
        Foreground="{StaticResource ThemeDefault.Red}"
        Text="For WPF" />
</Grid>
</code>
</pre>