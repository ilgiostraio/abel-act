   M S B A M L   `   `   `  ���� U  QAct.Lib.ControllersLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullkLclr-namespace:Act.Lib.ControllersLibrary;assembly=Act.Lib.ControllersLibraryAct.Lib.ControllersLibrary   Act.Face.FACEGui20"   Act.Face.FACEGui20.LookAtWin   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     ^localLclr-namespace:Act.Lib.ControllersLibrary;assembly=Act.Lib.ControllersLibrary     c _Microsoft.Expression.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35>edd4http://schemas.microsoft.com/expression/2010/drawing  -     � Title$  	LookAtWin��$	��430��$	��460������  �� EndPoint$ 0.5,1&� �� 
StartPoint$ 0.5,0&�.���  � Color$ 	#FF464646�� � Offset$ 0Q�� $ 	#FFFFFFFF��$ 1Q�.��� -   $��MainGrid��.+��� -    ��Name$ border��$��0��._��� $��Center=�$��Center=�
���   �$��8q� �� CornerRadius$ 8��
������� J� Effect 3  -System.Windows.Media.Effects.DropShadowEffect     Color$ White��	   
BlurRadius$	 6Q�
   ShadowDepth$
 0Q�._��� -    ��Name$ 	ECSCanvas��$	��400��$	��400��$��Center=�$��Center=�����  �� ImageSource$ Images\ECSBackground.png��.+�-  'Microsoft.Expression.Controls.LineArrow  -     Name$ Yaxis��2  ,Microsoft.Expression.Controls.CompositeShape   Fill �	#FFF4F4F5   Stroke
 �   �   StartCorner$ 
BottomLeft=�$��	200,0,0,0q����� *ActualHeight+ �� ElementName$ 	ECSCanvas�� �� Mode$ OneWay=�  -   $ Xaxis�� �	#FFF4F4F5$��0��
 �   �$ 
BottomLeft=�$��	0,200,0,0q����� *ActualWidth+$ 	ECSCanvas��0  *Microsoft.Expression.Shapes.RegularPolygon  -     Name$ Position_Star��$��12��   InnerRadius$ 0.47211Q�   
PointCount$ 5Q� �� Stretch$
 Fill=�
���  ��$��12��$
��Left=�$��Bottom=� J� OpacityMask
 �   �
���  �� K� ZIndex$ 10���� -    ��Name$ CurrentECSLabel��$��(0,0)�� �� Left$ 3��
 �� Top$ 5��
���   �