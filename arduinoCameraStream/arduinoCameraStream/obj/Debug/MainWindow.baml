   M S B A M L   `   `   `  ����  arduinoCameraStream$    arduinoCameraStream.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   J FWpfAnimatedGif, Version=1.4.12.0, Culture=neutral, PublicKeyToken=null,gif"http://wpfanimatedgif.codeplex.com  5   	     � Title$  Arduino Camera Viewer��5   	   $
��1085��6'   $	��735��64    � 	Resources 5         noBackground �� 
TargetType �� Property �� Value �� 
TargetType  RecordOn  	RecordOff  Stop  Play 	 Pause  SkipForward  SkipBackward 	 Eject��%�  &	        &	 �     &	     &	 5    &	 j    &	 �    &	 �    &	 �    &	 *    �� 5   
   " ��6%   .�5      ��  � �� 5	      �� 5
      " ��6&   .R�5      �� $��1q�6!   ���#b2d7fe���#68a8e9._�5      �� $��2q�6/   5      5      5      5      �� 5   
   $��resources/RecordOn.png��6!   �� 5   
   $��resources/RecordOff.png��6"   �� 5   
   $!��resources/stopRecording.png��6   �� 5   
   $��resources/play.png��6   �� 5   
   $��resources/pause.png��6   �� 5   
   $��resources/skipForward.png��6$   �� 5   
   $ ��resources/skipBackward.png��6%   �� 5   
   $��resources/eject.png��6   5       J� CommandBindings 5      )  #System.Windows.Input.CommandBinding  5   
   -   5           Command$	 New��5      6?     5   
   -   5        $
 Open��5      6A     5   
   -   5        $
 Save��5      6A     5   
   -   5        $ SaveAs��5      6E   .��5!      � -   5         �Name$ mainGrid��5!      	 � RowDefinitions	 5"   
   �� 5#      $
��Auto �6   �� 5$      $��* �6   .+�5&   
   ]� $��0��6   .+�5'      o� 
 o� 
IsMainMenu
 . $	��Top=�6%   .@�5(      m� $��_File��6   .@�5)      m� $
��_New��6    m� Command$	 New��6-    m� Icon 5*      �� 5+      $ ��resources/NewFile_6276.png��6$   5-      m� 5.      $��_Open��6   $
 Open��6.    5/      �� 50      $��resources/Open_6529.png��6$   52      m� 53      $��_Save��6   $
 Save��6.    54      �� 55      $��resources/Save_6530.png��6$   57      m� 58      $��Save _As...��6   $ SaveAs��64   59      m� 5:      $��_Edit��6   .@�5;      m� $
��_Cut��6   $	 Cut��6-    5<      �� 5=      $��resources/Cut_6523.png��6$   5?      m� 5@      $��_Copy��6   $
 Copy��6.    5A      �� 5B      $��resources/Copy_6524.png��6$   5D      m� 5E      $��_Paste��6   $ Paste��6/    5F      �� 5G      $��resources/Paste_6520.png��6$   5I      5J      m� 5K      $��_View��6   m� 5L      $��_Window��6   m� 5M      $��_Help��6   5N      `� 5O      $��Stretch=�6   $	��Top=�68   .�5P      c� .@�5Q      �� $	��New��6    � ToolTip$ Open new file��6+   .��5R      �� $ ��resources/NewFile_6276.png��6    5S      �� 5T      $
��Open��6   $ Open existing file��6,   .��5U      �� $��resources/Open_6529.png��6    5V      �� 5W      $
��Save��6   $ Save current file��6,   .��5X      �� $��resources/Save_6530.png��6    5Y      5Z      c� 5[      .@�5\      � .+�5]      �� -   5         ��Name$ COMList��5]   #   $	��290��64   $��0,0,0,0q�6@   {�.  6Q   �� 5^      -   5         ��Name$ COMListLoadingSpinner��5^       "  WpfAnimatedGif.ImageBehavior   RepeatBehavior$ Forever��6?      AnimatedSource$ resources/w8spinner.gif��6j   5_      �� 5`      -   5         ��Name$ openSerialSettings��5`      $��Settings��69   $��75��6L   $��5,0,0,0q�6W   
�������{�.  6{   �� 5a      -	   5        $ 
openSerial��5a      $
��Open��61   $��75��6@   $��5,0,0,0q�6K   
�������{�.  6o   5b      c� 5c      .@�5d      �� -
   5        $ 
openStream��5d      $ Play a raw camera stream��61   #���  6T   �� 5e      �� 5f      -   5        $ skipBackward��5f      $  Record a raw camera stream��63   #���  6X   �� 5g      -   5        $ record��5g      $  Record a raw camera stream��6-   #���  6R   �� 5h      -   5        $ 	playPause��5h      $ Play a raw camera stream��60   #���  6S   �� 5i      -   5        $ skipForward��5i      $  Record a raw camera stream��62   #���  6W   5j      c� 5k      .@�5l      � $	��200��6   .+�5m      � -   5         �Name$ serialProgress��5m   &    � Value$ 0Q�6>   �� 5n      -   5         ��Name$ serialProgressText��5n   $   $��Center=�6@   $��Center=�6]   
0/0 (0%)5p      5q      5r      5s      5t      � 5u   
   $��1��6   $��5,5,5,5q�6    � ColumnDefinitions 5v      �� 5w      $	��200 �6#   �� 5x      $��5 �6#   �� 5y      $��* �6#   	 5{      �� 5|      $	��480 �6    �� 5}      $��* �6    .+�5      �� -   5         ��Name$ readingListView��5      $��0��60   $��0��6=   ��6M   �� *
readings+ �� View 5�      �� 5�      .P�5�      �� $��Register��6S    �� Width$ 97��6e    �� DisplayMemberBinding 6)   �� *
register+6e   �� 5�      $��Value��6U   $ 97��6d    6)   �� *
lastRecord+6d   5�      5�      �� 5�      $��0��6   $��1��6(   $��Stretch=�68   $��5��6V   � 5�      $��0��6   $��2��6    .+�5�      �� }�. ._�5�      �� -   5         ��Name$ uiCanvas��5�      
������� � Cursor$ resources/cross.cur��6A   $	��640��6^   $	��480��6j   5�      5�      �� 5�      -   5         ��Name$ 
rTBConsole��5�      $��3��6.   $��1��6B   " �� VerticalScrollBarVisibility$ Visible=�6O   $��0,5,0,0q�6u    �� 
IsReadOnly . 6�   5�      5�      5�      