   M S B A M L   `   `   `  ����   Act.Lib.ControllersLibrary.    (Act.Lib.ControllersLibrary.ECSController   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     c _Microsoft.Expression.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35=ed4http://schemas.microsoft.com/expression/2010/drawing  -     � 	Resources   �� MergedDictionaries �� Source��  �� $_ Ypack://application:,,,/Act.Lib.ControllersLibrary;component/Resources/ContainerStyle.xaml?�%    .���� -    ��Name$ border��   ecsborderstyle#��[  ._��� $��Center=�$��Center=�
���   �$��8q� �� CornerRadius$ 8��
������� J� Effect 3  -System.Windows.Media.Effects.DropShadowEffect     Color$ White��   
BlurRadius$ 6Q�   ShadowDepth$ 0Q�._��� -   	 ��Name$	 	ECSCanvas��$	��360��$	��360��$��Center=�$��Center=����� 
 �� ImageSource$"
 Images\ECS\ECSBackground.png��.+�-  'Microsoft.Expression.Controls.LineArrow  -     Name$ Yaxis��2  ,Microsoft.Expression.Controls.CompositeShape   Fill �	#FFF4F4F5   Stroke
 �   �   StartCorner$ 
BottomLeft=�$��	180,0,0,0q����� *ActualHeight+ �� ElementName$ 	ECSCanvas�� �� Mode$ OneWay=�  -   $ Xaxis�� �	#FFF4F4F5$��0��
 �   �$ 
BottomLeft=�$��	0,180,0,0q����� *ActualWidth+$ 	ECSCanvas��0  *Microsoft.Expression.Shapes.RegularPolygon  -     Name$ Position_Star��$��12��   InnerRadius$ 0.47211Q�   
PointCount$ 5Q� �� Stretch$
 Fill=�
���  ��$��12��$
��Left=�$��Bottom=� J� OpacityMask
 �   �
���  �� K� ZIndex$ 10���� -    ��Name$ CurrentECSLabel��$��(0,0)�� �� Left$ 3��
 �� Top$ 5��
���   �