   M S B A M L   `   `   `  ����  Act.Face.FACEGui20$    Act.Face.FACEGui20.ErrorDialog   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  ErrorDialog��5   	   $
��Auto��6   $	��380��6+    J� SnapsToDevicePixels .  � SizeToContent$ Height=�5   $    � WindowStartupLocation$ CenterScreen=�5   	   $��0,0,0,0q�5   	    � WindowStyle$ ThreeDBorderWindow=�6    � Icon$&  /Images/DialogBoxes/Error-32.png��5   	   .��5
      �� -   5         ��Name$ borderCustomDialog��5
       �� CornerRadius$ 2,2,2,2��6?    �� Padding$ 0,0,0,0q�6V   ��6)   Z� ._�5   
   �� $
��Auto��6   $
��Auto��6"   .+�5      � 
�������$
��Auto��6&   $
��Auto��63   $��0,-1,0,0q�6A   	 � ColumnDefinitions	 5      �� 5      $
��Auto �6'   �� 5      
 � RowDefinitions
 5      �� 5      $
��Auto �6$   �� 5      $
��Auto �6$   �� 5      $
��Auto �6$   �� 5      $
��Auto �6$   �� 5      $
��Auto �6$   .+�5      �� -   5         ��Name$ imgInstructionIcon��5      $
��Left=�64   $��	10,10,8,8q�6O   $&�� /Images/DialogBoxes/Error-32.png��6b   $��32��6�   $��34��6�   $��1��6�   $��0��6�   �� 5      -   5         ��Name$ tbInstructionHeading��5      $��Stretch=�6:   $��Center=�6X   $
��Auto��6s   $��Error��6�   $
��Wrap=�6�   $��1��6�   $��1��6�   $��1��6�   $��Arial)�6�   $��18(�6�   ���	#FF006ECD6�   �� 5      -   5        $ tbInstructionText��5      $��0,0,8,0q�67   $��Stretch=�6H   $
��Auto��6f   $��Arial)�6s   $�� ��6�   $
��Wrap=�6�   $��1��6�   $��1��6�   $��1��6�   $��1��6�   �� 5      -   5        $ tbAdditionalDetailsText��5      $��Stretch=�6=   $
��Auto��6Y   $��Additional Details Text��6g   $
��Wrap=�6�   $��0,10,0,0q�6�   $��Arial)�6�   $��1��6�   $��2��6�   $y�	Collapsed=�6�   �� 5      $��0,10,0,0q�6   $	��Top=�6+   $
��Auto��6C   ���	#FFECECEC���	#FFC9C9C9$��0,1,0,1q�6�   $ 8,0,0,0q�6�   $��2��6�   $��3��6�   ._�5      ]� $��0,8,0,8q�6    $
��Auto��61   $
��Auto��6>   ��6L   Z� .+�5      �� -   5         ��Name$ errorDialogOkButton��5   !      ButtonStyle#��[  6`   $��26��6�   $��75��6�   $��Ok��6�   $��Right=�6�   $��0,0,8,0q�6�   $��Center=�6�   $��Right=�6�   5      5       5!      5"      5#      5%      