   M S B A M L   `   `   `  ����U  QAct.Lib.ControllersLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullkLclr-namespace:Act.Lib.ControllersLibrary;assembly=Act.Lib.ControllersLibraryAct.Lib.ControllersLibrary  5   	    Act.Face.FACEGui20"   Act.Face.FACEGui20.LookAtWin   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   ^localLclr-namespace:Act.Lib.ControllersLibrary;assembly=Act.Lib.ControllersLibrary     5   	   c _Microsoft.Expression.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35>edd4http://schemas.microsoft.com/expression/2010/drawing  5   	   -   5          � Title$  	LookAtWin��5   	   $	��430��6   $	��460��6'   ��5	      �� 5
   
    �� EndPoint$ 0.5,1&�6    �� 
StartPoint$ 0.5,0&�6/   .��5      �  � Color$ 	#FF464646��6    � Offset$ 0Q�6-   � 5      $ 	#FFFFFFFF��6   $ 1Q�6-   5      .��5      � -   5        $��MainGrid��5      .+�5   
   �� -   5         ��Name$ border��5      $��0��6!   ._�5      �� $��Center=�6   $��Center=�62   
���   �$��8q�6a    �� CornerRadius$ 8��6u   
������� J� Effect 5      3  -System.Windows.Media.Effects.DropShadowEffect  5         Color$ White��6'   	   
BlurRadius$	 6Q�65   
   ShadowDepth$
 0Q�6D   6V   ._�5      �� -   5         ��Name$ 	ECSCanvas��5      $	��400��6,   $	��400��68   $��Center=�6E   $��Center=�6b   ��5      �� 5       �� ImageSource$ Images\ECSBackground.png��6%   .+�5      -  'Microsoft.Expression.Controls.LineArrow  -   5          Name$ Yaxis��5   $   2  ,Microsoft.Expression.Controls.CompositeShape   Fill �	#FFF4F4F5   Stroke
 �   �   StartCorner$ 
BottomLeft=�6S   $��	200,0,0,0q�6l   ��6�   �� *ActualHeight+ �� ElementName$ 	ECSCanvas�� �� Mode$ OneWay=�  5      -   5        $ Xaxis��5   $    �	#FFF4F4F5$��0��6D   
 �   �$ 
BottomLeft=�6^   $��	0,200,0,0q�6w   ��6�   �� *ActualWidth+$ 	ECSCanvas��0  *Microsoft.Expression.Shapes.RegularPolygon  5      -   5          Name$ Position_Star��5   )   $��12��6@      InnerRadius$ 0.47211Q�6L      
PointCount$ 5Q�6b    �� Stretch$
 Fill=�6q   
���  ��$��12��6�   $
��Left=�6�   $��Bottom=�6�    J� OpacityMask
 �   �
���  �� K� ZIndex$ 10��6�   �� 5      -   5         ��Name$ CurrentECSLabel��5      $��(0,0)��65    �� Left$ 3��6E   
 �� Top$ 5��6U   
���   �6d   5      5       5"      5/      50      