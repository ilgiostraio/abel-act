   M S B A M L   `   `   `  ����  Act.Lib.ControllersLibrary.    (Act.Lib.ControllersLibrary.ECSController   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      c _Microsoft.Expression.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35=ed4http://schemas.microsoft.com/expression/2010/drawing  5      -   5          � 	Resources  5	       �� MergedDictionaries �� Source�� 5
   
    5      �� 5      $_ Ypack://application:,,,/Act.Lib.ControllersLibrary;component/Resources/ContainerStyle.xaml?�6%   %    5      .��5      �� -   5         ��Name$ border��5         ecsborderstyle#��[  6   ._�5   
   �� $��Center=�6   $��Center=�6.   
���   �$��8q�6]    �� CornerRadius$ 8��6q   
������� J� Effect 5      3  -System.Windows.Media.Effects.DropShadowEffect  5         Color$ White��6#      
BlurRadius$ 6Q�61      ShadowDepth$ 0Q�6@   6R   ._�5      �� -   5        	 ��Name$	 	ECSCanvas��5      $	��360��6(   $	��360��64   $��Center=�6A   $��Center=�6^   ��5      �� 5      
 �� ImageSource$"
 Images\ECS\ECSBackground.png��6!   .+�5      -  'Microsoft.Expression.Controls.LineArrow  -   5          Name$ Yaxis��5      2  ,Microsoft.Expression.Controls.CompositeShape   Fill �	#FFF4F4F5   Stroke
 �   �   StartCorner$ 
BottomLeft=�6N   $��	180,0,0,0q�6g   ��5      �� *ActualHeight+ �� ElementName$ 	ECSCanvas�� �� Mode$ OneWay=�  5      -   5        $ Xaxis��5       �	#FFF4F4F5$��0��6?   
 �   �$ 
BottomLeft=�6Y   $��	0,180,0,0q�6r   ��5      �� *ActualWidth+$ 	ECSCanvas��0  *Microsoft.Expression.Shapes.RegularPolygon  5      -   5          Name$ Position_Star��5   $   $��12��6;      InnerRadius$ 0.47211Q�6G      
PointCount$ 5Q�6]    �� Stretch$
 Fill=�6l   
���  ��$��12��5    $   $
��Left=�60   $��Bottom=�6K    J� OpacityMask
 �   �
���  �� K� ZIndex$ 10��6�   �� 5!      -   5         ��Name$ CurrentECSLabel��5!      $��(0,0)��61    �� Left$ 3��6A   
 �� Top$ 5��6Q   
���   �6`   5"      5#      5$      5%      