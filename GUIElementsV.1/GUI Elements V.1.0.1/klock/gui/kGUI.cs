/* Paul Knab - www.k-lock.de - 2011 - kGUI - A extended Unity EditorGUI Class - V.1.2*/
/*---------------------------------------------------------------------------------------------- 
 UPDATES

 15.09.2012
    Setup Drawing method for better positioning elements
    DrawLabel - now with guistyle
 14.09.2012
    kComboPop - guistyle 
 10.09.2012
 	kMenuElement - menu componente using genetic menu for illustrated menu icons
 	 
 07.09.2012
 	static Size for textfield elements (text label list )
 	
 06.09.2012
 	BTN_WIDTH - for NumStep, ComboBox, PopUp
 	
 05.09.2012
 	Color Settings
 	ResetColors()
 
 05.09.2012
 	SINGLE_LINE -> Layout option to draw a single or double outline.
 
 05.09.2012
 	Hot Control fixes. 
 	Doto -kComboBox open
 	 
 29.08.2012
 	kDbtn -> a Drag button.
 	
 25.08.2012
 	kComboPop( bool left = false ) -> [ left ] Set now the layout orientation mode. 
 	
 26.08.2012
 	kMenuParent -> A menu component.
 ----------------------------------------------------------------------------------------------*/
using klock.drawing;

using UnityEditor;
using UnityEngine;

namespace klock.gui
{
	/** kGUI - A extended Unity EditorGUI Class */
    public class kGUI
    {	
		#region Menu Element
		//-----------------------------------------------------------------------------
        //
        // 																Menu Element
        //
        //-----------------------------------------------------------------------------
		/** A menu componente using the unity GenericMenu class.

		* @param Rect bounds - The position and size of this element.
		* @param GenericMenu menu - A menu that contains the item data.
		* 
		* For return data use the callback function of the GenericMenu class.*/
		public static void kMenuElement( Rect bounds, string label, GenericMenu menu)
		{
			bool inBounds = bounds.Contains(Event.current.mousePosition);
			int controlID = GUIUtility.GetControlID(bounds.GetHashCode(), FocusType.Native);
			bool isDown   = GUIUtility.hotControl == controlID;
			
			EventType et = Event.current.GetTypeForControl(controlID);
			switch (et)
			{
				 case EventType.mouseDown:

                    if ( inBounds && GUIUtility.hotControl == 0 )
					{ 
						SetHotControl(inBounds, controlID, 0);
					}

                break;
                case EventType.mouseUp:

                    if ( isDown && inBounds ) 
					{	
						SetHotControl(true, 0, 0);
					}
				
                    break;
			    case EventType.repaint:
   
			        GUIStyle style = textStyle; 
							 style.normal.textColor  = new Color(.3f, .3f, .3f);
				
					if( inBounds )
					{
						kDraw.FillRect(new Rect(bounds.x, bounds.y, bounds.width, bounds.height), 1, new Color(1, 1, 1, .3f), C_BorderColor);
	        		   	style.normal.textColor  = Color.black;
					}
				
					style.Draw( bounds, new GUIContent(label), controlID);

				break;
			}

			if ( inBounds && isDown ) 
			{
				menu.DropDown( bounds );
				Event.current.Use();
			}
		}
		/** A simple menu component without a label.

		* @param Rect bounds - The position and size of this element.
		* @param string[] content - A list contains the item data. 
		* 
		* @returns int - The current value of the selected index. */
		public static int kMenuParent( Rect bounds, string[] content )
		{
			int controlID = GUIUtility.GetControlID(bounds.GetHashCode(), FocusType.Native);

			switch (Event.current.GetTypeForControl(controlID))
			{
			    case EventType.repaint:
 
					textStyle.Draw( bounds, new GUIContent(""), controlID);

				break;
			}
			
			int selected = EditorGUI.Popup( bounds, -1, content, textStyle );
			
			return selected;
		}
		/** A simple menu component with label.

		* @param Rect bounds - The position and size of this element.
		* @param string 	label - The label text to display.
		* @param string[] content - A list contains the item data. 
		* 
		* @returns int - The current value of the selected index. */
		public static int kMenuParent( Rect bounds, string label, string[] content )
		{	
			int controlID = GUIUtility.GetControlID(bounds.GetHashCode(), FocusType.Native);
			bool inBounds = bounds.Contains(Event.current.mousePosition);

			switch (Event.current.GetTypeForControl(controlID))
			{
			    case EventType.repaint:
   
			        GUIStyle style = textStyle; style.normal.textColor  = new Color(.3f, .3f, .3f);
				
					if( inBounds )
					{
						kDraw.FillRect(new Rect(bounds.x, bounds.y, bounds.width, bounds.height), 1, new Color(1, 1, 1, .3f), C_BorderColor);
	        		   	style.normal.textColor  = Color.black;
					}
				
					style.Draw( bounds, new GUIContent(label), controlID);

				break;
			}
			
			int selected = EditorGUI.Popup( bounds, -1, content, new GUIStyle() );
			
			return selected;			
		}
		#endregion
		#region ComboBox
		//-----------------------------------------------------------------------------
        //
        // 																ComboBox
        //
        //-----------------------------------------------------------------------------
		/** A Unity GUI.PopUp ComboBox component. With a labelfield infront of the element.
		* Using a genetic menu for a better z sort if more elements in use.
		*
		* @param Rect bounds - The position and size of this element.
		* @param string[] content - A list contains the item data. 
		* @param int selected - The selecteed index of the item list.
		* @param bool left - Set the layout orientation mode for the button. (left or right)
		* 
		* @returns int - The current value of the selected index.*/
		public static int kComboPop(Rect bounds, string[] content, int selected, GUIContent label, bool left = false )
        {
			DrawLabel( bounds, label );

			return kComboPop( new Rect( bounds.x + LABEL_WIDTH, bounds.y, bounds.width, bounds.height ), content, selected, left );
		}
        /** A Unity GUI.PopUp ComboBox component. Using a genetic menu for a better z sort if more elements in use.
        *
        * @param Rect bounds - The position and size of this element.
        * @param string[] content - A list contains the item data. 
        * @param int selected - The selected index of the item list.
        * @param bool left - Set the layout orientation mode for the button. (left or right)
        * @param GUIStyle style - GUIStyle for the textField;
        * 
        * @returns int - The current value of the selected index.*/
        public static int kComboPop( Rect bounds, string[] content, int selected, bool left = false, GUIStyle style = null  )
		{
			int  controlID   = GUIUtility.GetControlID(bounds.GetHashCode(), FocusType.Native);
            bool inBounds    = bounds.Contains(Event.current.mousePosition);
			bool isDown = GUIUtility.hotControl == controlID;

			float btnW = BTN_WIDTH;
            //float txtW = bounds.width - btnW + 1; 
            Rect  btn  = new Rect((left)?bounds.x:bounds.x +bounds.width - btnW, bounds.y, btnW, bounds.height);
            Rect  txt  = new Rect((left)?bounds.x+btnW-1:bounds.x, bounds.y, bounds.width - btnW +1, bounds.height);
            
      //      style = textStyle; style.alignment = (left) ? TextAnchor.MiddleRight : TextAnchor.MiddleLeft;
     //       if (style != null) _style = style;

            switch ( Event.current.GetTypeForControl(controlID) )
			{			
				case EventType.repaint:
		
					//( inBounds) ? C_BorderActive : C_BorderColor, C_BorderColor2);
					if(SINGLE_LINE){
						kDraw.FillRect( txt, 1, C_TxtField, ( inBounds) ? C_BorderActive : C_BorderColor);
						kDraw.FillRect( btn, 1, (isDown) ? C_FillActive : (inBounds) ? C_FillRoll : C_FillColor, (inBounds) ? C_BorderActive : C_BorderColor );
					}else{
						kDraw.FillRect( txt, 2, C_TxtField, ( inBounds) ? C_BorderActive : C_BorderColor, C_BorderColor2);
						kDraw.FillRect( btn, 2, (isDown) ? C_FillActive : (inBounds) ? C_FillRoll : C_FillColor, (inBounds) ? C_BorderActive : C_BorderColor, C_BorderColor2);
					}
					textStyle.Draw( btn, new GUIContent( kGFXTexture.Triangle(1, Color.grey) ), controlID);
				
				break;
			}
			
			selected = EditorGUI.Popup( bounds, selected, content, textStyle );
           		
			return selected;
		}
		/** A GUI comboBox component. With a labelfield infront of the element.
		*
		* @param Rect bounds - The position and size of this element.
		* @param object[] content - A list contains the item data. 
		* @param int selected - The selecteed index of the item list.
		* @param bool left - Set the layout orientation mode for the button. (left or right)
		* 
		* @returns int - The current value of the selected index.*/
		public static int kComboBox(Rect bounds, object[] content, int selected, GUIContent label, bool left = false )
        {
			DrawLabel( bounds, label );

			return kComboBox( new Rect( bounds.x + LABEL_WIDTH, bounds.y, bounds.width, bounds.height ), content, selected, left);
		}
		/** A GUI comboBox component.
		*
		* @param Rect bounds - The position and size of this element.
		* @param object[] content - A list contains the item data. 
		* @param int selected - The selecteed index of the item list.
		* @param bool left - Set the layout orientation mode for the button. (left or right) 
		* 
		* @returns int - The current value of the selected index.*/
        public static int kComboBox( Rect bounds, object[] content, int selected, bool left = false )
        {
             int controlID   = GUIUtility.GetControlID(bounds.GetHashCode(), FocusType.Native);
			 int keyboardID  = GUIUtility.keyboardControl;
           // bool inBounds    = bounds.Contains(Event.current.mousePosition);
 			bool listHot     = GUIUtility.hotControl == GUIUtility.keyboardControl &&  GUIUtility.hotControl != 0 && GUIUtility.hotControl == controlID;
			
			float btnW = BTN_WIDTH;
			Rect  btn  = new Rect((left)?bounds.x:bounds.x +bounds.width - btnW, bounds.y, btnW, bounds.height);
            Rect  txt  = new Rect((left)?bounds.x+btnW-1:bounds.x, bounds.y, bounds.width - btnW +1, bounds.height);
           
			kLabel( txt, new GUIContent ( content[selected].ToString() ));

			if( kRbtn( btn, new GUIContent( kGFXTexture.Triangle(1, Color.grey))) && Event.current.GetTypeForControl( controlID ) == EventType.mouseUp)
			{
				if( keyboardID == controlID && controlID != 0 ) //listHot = false
				{
					SetHotControl( true, 0, 0 );
	
				}else if( GUIUtility.hotControl == 0 )			//listHot = true
				{
					SetHotControl( true, controlID, controlID );
	
				}
			}				
		
			if( listHot )
			{
				int controlSaver = GUIUtility.hotControl;
				
				SetHotControl( true, 0, -10 );

				int nSelect = kList( new Rect( bounds.x, bounds.y+bounds.height, bounds.width, bounds.height), content, selected );
				
				SetHotControl( true, controlSaver, controlID );	
			
				if( selected != nSelect ) 
				{
					SetHotControl( true, 0, 0 );
					return selected = nSelect;	
				}
			}

			return selected;
        }	
		/** List element. Can contain text or image items.
		* 
		* @param Rect bounds - The position and size. ( ! Position of the first item in the list )
		* @param object[] content - A list contains the item data. 
		* @param int selected - The selecteed index of the item list.
		* 
		* @returns int - The current value of the selected index from the item list.*/
		public static int kList( Rect bounds, object[] content, int selected )
		{
			int   listCount  = content.Length;
			float listHeight = 19 * listCount;
			float itemWidth  = bounds.width-4;
			float itemHeight = 18;
			Rect  listBox    = new Rect( bounds.x, bounds.y-1, bounds.width, listHeight+3);
			int   listID     = GUIUtility.GetControlID( listBox.GetHashCode(), FocusType.Passive, listBox);
		
			switch ( Event.current.GetTypeForControl(listID) )
			{
				case EventType.mouseDown:
				case EventType.repaint:

					kDraw.FillRect( listBox, 2, C_TxtField, C_BorderColor, C_BorderColor2);	

					for( int i=0; i<listCount; i++ )
					{	
						Rect iBounds = new Rect(  bounds.x+2, bounds.y+( i * 19 )+1,itemWidth, itemHeight );

						if( i == selected ) kDraw.FillRect( iBounds,  new Color(0, .4f, .8f, .2f) );
						if( kListItem( iBounds, new GUIContent( content[i].ToString()) ))
						{
							selected = i;
						}
					}
				
				break;
			}
		
			return selected;
		}
		/** ListItem for an list element.
		* 
		* @param Rect bounds - The position and size.
		* @param GUIContent content  - A GUIContent for display a label object that can contain text, tooltip or a image.
		* 
		* @returns bool - Retruns true if user click left mouse on it.*/
		public static bool kListItem( Rect bounds, GUIContent content )
        {
        	//int controlID = GUIUtility.GetControlID(bounds.GetHashCode(), FocusType.Native, bounds);
            bool inBounds  = bounds.Contains(Event.current.mousePosition);

			switch (Event.current.type )
            {
            	case EventType.mouseDown:

					if ( inBounds )
					{
						SetHotControl( true, 0, 0);
					
						return true;
					}
                    break;

              	case EventType.repaint:
							
					listStyle.Draw( bounds, content, inBounds, false, false, false);
	
                    break;
            }
			
            return false;
        }
		#endregion
        //-----------------------------------------------------------------------------
        //
        // 																Numeric Stepper
        //
        //-----------------------------------------------------------------------------
        #region Num Stepper
		/** Numeric stepper with a range ( min and max )for the value. With a labelfield infront of the element.
		* And the ability to change the value with a mousewheel event.
		*
		* Click inside to set the keyboard focus to this element. And set the mouseWheel mode to true.
		* In the active mode a color border will show arround the element.
		* Press RETURN or click outside the element to deactivate the textfield focus.
		* @param Rect   bounds	- The gui rectangle to draw element.
		* @param float  _value	- The value number for the element.
		* @param float  _step	- The numeric size of a step.
		* @param float  min		- The minimum number for the element value.( default = 0 )
		* @param float  max		- The maximum number for the element value.( default = 10 )
		*
		* @returns float 	- A float value with the current value of this element.*/
		public static float kMstep(Rect bounds, float _value, float _step, GUIContent label = null, float min = 0, float max = 10 )
        {
            if (label != null) DrawLabel(bounds, label);

            return kMstep(new Rect(bounds.x + ((label != null) ? LABEL_WIDTH : 0), bounds.y, bounds.width, bounds.height), _value, _step, min, max);
		}
		/**
		* Numeric stepper with a range ( min and max )for the value.And the ability to change the value with a mousewheel event.
		*
		* Click inside to set the keyboard focus to this element. And set the mouseWheel mode to true.
		* In the active mode a color border will show arround the element.
		* Press RETURN or click outside the element to deactivate the textfield focus.
		* @param Rect   bounds	- The gui rectangle to draw element.
		* @param float  _value	- The value number for the element.
		* @param float  _step	- The numeric size of a step.
		* @param float  min		- The minimum number for the element value.( default = 0 )
		* @param float  max		- The maximum number for the element value.( default = 10 )
		*
		* @returns float 	- A float value with the current value of this element.*/
        public static float kMstep(Rect bounds, float _value, float _step = 1.0f, float min = 0, float max = 10 )
        {

             int controlID   = GUIUtility.GetControlID(bounds.GetHashCode(), FocusType.Native);
			 int keyboardID  = GUIUtility.keyboardControl;
            bool inBounds    = bounds.Contains(Event.current.mousePosition);
            bool hotKeyboard = ( GUIUtility.hotControl == 0 ) && ( controlID + 1 == keyboardID ) && ( GUIUtility.keyboardControl != 0 );
			float btnW = BTN_WIDTH;
			float btnH = 12;
             Rect btn1 = new Rect(bounds.x, bounds.y, btnW, btnH);
             Rect btn2 = new Rect(bounds.x, bounds.y + btnH - 1, btnW, btnH);
             Rect txt  = new Rect(bounds.x + btnW - 1, bounds.y, bounds.width - btnW + 1, bounds.height);

			switch ( Event.current.GetTypeForControl(controlID) )
            {

				case EventType.scrollWheel:

					if ( hotKeyboard )
					{
		                _value += (Event.current.delta.y / 3) * _step;

						GUIUtility.keyboardControl = 0;

					}else return SetRange( _value, min, max );


				break;
                case EventType.repaint:
					if(SINGLE_LINE)
						kDraw.FillRect(txt, 1, C_TxtField, (hotKeyboard || inBounds) ? C_BorderActive : C_BorderColor);
					else
                    	kDraw.FillRect(txt, 2, C_TxtField, (hotKeyboard || inBounds) ? C_BorderActive : C_BorderColor, C_BorderColor2);

                break;
                case EventType.mouseUp:

                    if (hotKeyboard)
                    {
						SetHotControl(true, 0, 0);
                        return _value;
                    }

                break;
            }
            _value = EditorGUI.FloatField(txt, _value, textStyle );

            if (kBtn(btn1, new GUIContent(kGFXTexture.Triangle(0, C_TxtField)), null, true)) _value += _step;
            if (kBtn(btn2, new GUIContent(kGFXTexture.Triangle(1, C_TxtField)), null, true)) _value -= _step;

            if ( hotKeyboard || inBounds ) kDraw.OutRect(bounds, C_BorderActive);
			if ( hotKeyboard ) GUIUtility.keyboardControl = controlID+1;


            return SetRange( _value, min, max );
        }
		/**
		* Numeric stepper with a range ( min and max )for the value.Numeric stepper with a range ( min and max )for the value. With a labelfield infront of the element.
		*
		* Click inside to set the keyboard focus to this element.
		* In the active mode a color border will show arround the element.
		* Press RETURN or click outside the element to deactivate the textfield focus.
		*
		* @param Rect bounds	- The gui rectangle to draw element.
		* @param float  _value	- The value number for the element.
		* @param float  _step	- The numeric size of a step.
		* @param GUIContent content  - A GUIContent for display a label object that can contain text, tooltip or a image.
		* @param float  min		- The minimum number for the element value. ( default = 0 )
		* @param float  max		- The maximum number for the element value. ( default = 10 )
		*
		*
		* @returns float 	- A float value with the current value of this element.*/
		public static float kRstep(Rect bounds, float _value, float _step, GUIContent label=null, float min = 0, float max = 10 )
        {
            if (label != null) DrawLabel(bounds, label);

			return kRstep( new Rect( bounds.x +((label!=null)?LABEL_WIDTH:0), bounds.y, bounds.width, bounds.height ), _value, _step, min, max );
		}
		/**
		* Numeric stepper with a range ( min and max )for the value.
		*
		* Click inside to set the keyboard focus to this element.
		* In the active mode a color border will show arround the element.
		* Press RETURN or click outside the element to deactivate the textfield focus.
		*
		* @param Rect   bounds	- The gui rectangle to draw element.
		* @param float  _value	- The value number for the element.
		* @param float  _step	- The numeric size of a step.
		* @param float  min		- The minimum number for the element value.( default = 0 )
		* @param float  max		- The maximum number for the element value.( default = 10 )
		*
		* @returns float 	- A float value with the current value of this element.*/
        public static float kRstep(Rect bounds, float _value, float _step, float min = 0, float max = 10)
        {
            return SetRange(kStep(bounds, _value, _step, null), min, max);
        }
		/** Standart numeric stepper. With a labelfield infront of the element.
		*
		* Click inside to set the keyboard focus to this element.
		* In the active mode a color border will show arround the element.
		* Press RETURN or click outside the element to deactivate the textfield focus.
		*
		* @param Rect bounds	- The gui rectangle to draw element.
		* @param float  _value	- The value number for the element.
		* @param float  _step	- The numeric size of a step.
		* @param GUIContent content  - A GUIContent for display a label object that can contain text, tooltip or a image.
		*
		* @returns float 	- A float value with the current value of this element.*/
		public static float kStep(Rect bounds, float _value, float _step, GUIContent label)
        {
			if(label!=null)DrawLabel( bounds, label );

            return kStep(new Rect(bounds.x + ((label != null) ? LABEL_WIDTH : 0), bounds.y, bounds.width, bounds.height), _value, _step);
		}
		/**
		* Standart numeric stepper.
		*
		* Click inside to set the keyboard focus to this element.
		* In the active mode a color border will show arround the element.
		* Press RETURN or click outside the element to deactivate the textfield focus.
		*
		* @param Rect bounds	- The gui rectangle to draw element.
		* @param float  _value	- The value number for the element.
		* @param float  _step	- The numeric size of a step.
		*
		* @returns float 	- A float value with the current value of this element.*/
        public static float kStep(Rect bounds, float _value, float _step)
        {
             int controlID   = GUIUtility.GetControlID(bounds.GetHashCode(), FocusType.Native);
			 int keyboardID  = GUIUtility.keyboardControl;
            bool inBounds    = bounds.Contains(Event.current.mousePosition);
            bool hotKeyboard = ( GUIUtility.hotControl == 0 ) && ( controlID + 1 == keyboardID ) && ( GUIUtility.keyboardControl != 0 );
			float btnW = BTN_WIDTH;
			float btnH = 12;
             Rect btn1 = new Rect(bounds.x, bounds.y, btnW, btnH);
             Rect btn2 = new Rect(bounds.x, bounds.y + btnH - 1, btnW, btnH);
             Rect txt  = new Rect(bounds.x + btnW - 1, bounds.y, bounds.width - btnW + 1, bounds.height);

            switch (Event.current.GetTypeForControl(controlID))
            {

                case EventType.repaint:
					if(SINGLE_LINE)
                    	kDraw.FillRect(txt, 1, C_TxtField, (hotKeyboard || inBounds) ? C_BorderActive : C_BorderColor);
					else
                    	kDraw.FillRect(txt, 2, C_TxtField, (hotKeyboard || inBounds) ? C_BorderActive : C_BorderColor, C_BorderColor2);

                break;
                case EventType.mouseUp:

                    if (hotKeyboard)
                    {
                        SetHotControl(true, 0, 0);
                        return _value;
                    }

                break;
            }
            _value = EditorGUI.FloatField(txt, _value, textStyle);

            if (kBtn(btn1, new GUIContent(kGFXTexture.Triangle(0, C_TxtField)), null, false)) _value += _step;
            if (kBtn(btn2, new GUIContent(kGFXTexture.Triangle(1, C_TxtField)), null, false)) _value -= _step;

            if (inBounds || hotKeyboard) kDraw.OutRect(bounds, C_BorderActive);

            return _value;
        }
        #endregion
        //-----------------------------------------------------------------------------
        //
        // 																	Button
        //
        //-----------------------------------------------------------------------------
        #region Button
		/** Drag Button.
		 *
		 * @param Rect 	bounds - The position and size of this element.
		 * @param Color color  - The colorof this element.
		 * @parma bool useX	   - The elements permit horizpntal transforms.
		 * @parma bool useY	   - The elements permit vertical transforms.
		 * 
		 * @returns Rect -  The current rect position and size. */
		public static Rect kDbtn( Rect bounds, Color color, bool useX = true, bool useY = true )
		{
			float 	fMouseX = Event.current.mousePosition.x;
			float 	fMouseY = Event.current.mousePosition.y;
			
		//	Vector2 center = new Vector2(bounds.x + bounds.width*.5f, bounds.y + bounds.height*.5f);
			
			int controlID = GUIUtility.GetControlID(FocusType.Native);
			bool inBounds = bounds.Contains(Event.current.mousePosition);		
			bool isDown = GUIUtility.hotControl == controlID;
				
			EventType et = Event.current.GetTypeForControl(controlID);
			switch (et)
			{
				case EventType.mouseDown:
					if (inBounds)
                    {
						SetHotControl(inBounds, controlID, 0);
						Event.current.Use ();
                    }
				break;
				case EventType.mouseUp:
					if (isDown)
					{
						SetHotControl(true, 0, 0);
						Event.current.Use ();
					}
				break;
				case EventType.mouseDrag:
					
					if (isDown)  bounds = new Rect( (useX) ? fMouseX-bounds.width*.5f : bounds.x , (useY) ? fMouseY-bounds.height*.5f : bounds.y, bounds.width, bounds.height );	
		
				break;
			    case EventType.repaint:
					kDraw.FillRect(bounds, (inBounds)?color:new Color(color.r,color.g,color.b, .2f));
				
				break;
			}
			return bounds;
		}

        /** Toggle Button with rollover/out and the ability to show a state of a boolean variable.  Shows a GUIContent[] caption. And with a labelfield infront of the element.
        *
        * @param Rect 	bounds - The position and size of this element.
        * @param  bool		state   - The state of the toggle.
        * @param GUIContent content  - A GUIContent for display a label object that can contain text, tooltip or a image.
        *
        * @returns boolean -  Returns true, if the user clicks the button. */
        public static bool kTbtn(Rect bounds, bool state, GUIContent content, GUIContent label = null )
        {
			if( label != null )DrawLabel( bounds, label );

            return kTbtn(new Rect(bounds.x + ((label != null)?LABEL_WIDTH:0), bounds.y, bounds.width, bounds.height), state, content, true, null );
		}
		/** Toggle Button with rollover/out and the ability to show a state of a boolean variable. Shows a string[] caption.
		*
		* @param Rect 	bounds - The position and size of this element.
		* @param  bool		state   - The state of the toggle.
		* @param string 	caption - The label text to display.
		* @param bool roll  - Set hover event active or false. To listen mouse Roll over and out.
		*
		* @returns boolean -  Returns true, if the user clicks the button. */
        public static bool kTbtn(Rect bounds, bool state, string caption, bool roll = true, GUIStyle style = null)
        {
            return kTbtn(bounds, state, new GUIContent(caption), roll, (style == null) ? textStyle : style);
        }
        /** Toggle Button with rollover/out and the ability to show a state of a boolean variable.  Shows a GUIContent[] caption.
        * @param Rect 	  bounds  - The position and size of this element.
        * @param  bool		  state    - The state of the toggle.
        * @param GUIContent content  - A GUIContent for display a label object that can contain text, tooltip or a image.
        * @param bool roll  - Set hover event active or false. To listen mouse Roll over and out.
        *
        * @returns boolean -  Returns true, if the user clicks the button. */
        public static bool kTbtn(Rect bounds, bool state, GUIContent content, bool roll = true, GUIStyle style = null  )
        {
            GUIStyle _style = ( style==null )? textStyle : style;
            int controlID = GUIUtility.GetControlID(bounds.GetHashCode(), FocusType.Native, bounds);
            bool inBounds = bounds.Contains(Event.current.mousePosition);
            bool isDown = GUIUtility.hotControl == controlID;

            switch (Event.current.GetTypeForControl(controlID))
            {

                case EventType.mouseDown:

                    if (inBounds)
                    {
                        SetHotControl(inBounds, controlID, 0);
                        state = !state;
						Event.current.Use ();
                    }

                    break;
                case EventType.mouseUp:

                    if (isDown) SetHotControl(true, 0, 0);
                    if (inBounds) return state;

                    break;
                case EventType.repaint:
					
					if(SINGLE_LINE)
						kDraw.FillRect(bounds, 1, (state) ? C_FillActive : (inBounds&&roll) ? C_FillRoll : C_FillColor, (inBounds&&roll) ? C_BorderActive : C_BorderColor );
					else
                    	kDraw.FillRect(bounds, 2, (state) ? C_FillActive : (inBounds&&roll) ? C_FillRoll : C_FillColor, (inBounds&&roll) ? C_BorderActive : C_BorderColor, C_BorderColor2);
				 	
                    _style.Draw(bounds, content, controlID); 

                    break;
            }

            return state;
        }
		/** Press button with rollover/out. With labelfield infront of the element.
		*
		* @param Rect   _bound  - The position and size of this element.
		* @param GUIContent content - A GUIContent for display a label object that can contain text, tooltip or a image.
		*
		* @returns boolean -  Returns true, if the user clicks the button. */
		public static bool kRbtn( Rect bounds, GUIContent content, GUIContent label )
        {
			DrawLabel( bounds, label );

			return kRbtn( new Rect( bounds.x + LABEL_WIDTH, bounds.y, bounds.width, bounds.height ), content );
		}
		/** Press button with rollover/out.
		*
		* @param Rect 	bounds - The position and size of this element.
		* @param string 	caption - The label text to display.
		*
		* @returns boolean -  Returns true, if the user clicks the button. */
        public static bool kRbtn(Rect bounds, string caption)
        {
            return kRbtn(bounds, new GUIContent(caption));
        }
		/** Press button with rollover/out.
		*
		* @param Rect 	  bounds  - The position and size of this element.
		* @param GUIContent content  - A GUIContent for display a label object that can contain text, tooltip or a image.
		*
		* @returns boolean -  Returns true, if the user clicks the button. */
        public static bool kRbtn(Rect bounds, GUIContent content)
        {
            GUIStyle style = textStyle;
            int controlID = GUIUtility.GetControlID(bounds.GetHashCode(), FocusType.Native, bounds);
            bool inBounds = bounds.Contains(Event.current.mousePosition);
            bool isDown = GUIUtility.hotControl == controlID;

            switch (Event.current.GetTypeForControl(controlID))
            {

                case EventType.mouseDown:

                    if (inBounds) SetHotControl(inBounds, controlID, 0);

                    break;
                case EventType.mouseUp:

                    if (isDown) SetHotControl(true, 0, 0);
                    if (inBounds) return true;

                    break;
                case EventType.repaint:
					if(SINGLE_LINE) 
                    	kDraw.FillRect(bounds, 1, (isDown) ? C_FillActive : (inBounds) ? C_FillRoll : C_FillColor, (inBounds) ? C_BorderActive : C_BorderColor);
					else
						kDraw.FillRect(bounds, 2, (isDown) ? C_FillActive : (inBounds) ? C_FillRoll : C_FillColor, (inBounds) ? C_BorderActive : C_BorderColor, C_BorderColor2);
					
                    style.Draw(bounds, content, controlID); 

                    break;
            }

            return false;
        }
        /** Simple button element. Displays a GUIContent object.
        *
        * @param Rect 	    bounds  - The position and size of this element.
        * @param GUIContent content - A GUIContent for display a label object that can contain text, tooltip or a image.
        * @param bool       rool    - En/Disable the mouse roll over ability. 
        *
        * @returns boolean -  Returns true, if the user clicks the button. */
        public static bool kBtn(Rect bounds, GUIContent content, bool roll = false )
        {
            return kBtn(bounds, content, null, roll);
		}
        /**  Simple button element. Display a string.
        * 
        * @param Rect 	    bounds   - The position and size of this element.
        * @param string     caption  - The label text to display.
        * @param bool       rool     - En/Disable the mouse roll over ability. 
        *
        * @returns boolean -  Returns true, if the user clicks the button. */
        public static bool kBtn(Rect bounds, string caption, bool roll = false )
        {
            return kBtn(bounds, new GUIContent(caption), null, roll );
        }
        /** Simple button element.
        *
        * @param Rect 	    bounds  - The position and size of this element.
        * @param GUIContent content - A GUIContent to display a object that can contain text, tooltip or a image.
        * @param GUIContent label   - A GUIContent to display a label in front of the element. Can contain text, tooltip or a image.
        * @param bool       rool    - En/Disable the mouse roll over ability. 
        *
        * @returns boolean -  Returns true, if the user clicks the button. */
        public static bool kBtn(Rect bounds, GUIContent content, GUIContent label, bool roll, GUIStyle lStyle = null, GUIStyle bStyle = null)
        {
            if (label != null) 
            {
                DrawLabel( bounds, label, lStyle );
                bounds = new Rect(bounds.x + LABEL_WIDTH, bounds.y, bounds.width, bounds.height);
            }

            int controlID = GUIUtility.GetControlID(bounds.GetHashCode(), FocusType.Native, bounds);
            bool inBounds = bounds.Contains(Event.current.mousePosition);
            bool isDown = GUIUtility.hotControl == controlID;
            GUIStyle _style = (bStyle != null) ? bStyle : textStyle;

            switch (Event.current.GetTypeForControl(controlID))
            {

                case EventType.mouseDown:

                    if ( inBounds && GUIUtility.hotControl == 0 ) SetHotControl(inBounds, controlID, 0);

                    break;
                case EventType.mouseUp:

                    if ( isDown && inBounds ) 
					{	
						SetHotControl(true, 0, 0);
                    	return true;
					}

                    break;
                case EventType.repaint:
 
                    RenderElement(bounds, isDown, inBounds, roll);

                    _style.Draw(bounds, content, controlID); 
					
                    break;
            }

            return false;
        }
        private static void RenderElement(Rect bounds, bool isDown, bool inBounds, bool roll)
        {
            if (SINGLE_LINE)
                kDraw.FillRect(bounds, 1, (isDown) ? C_FillActive : ((inBounds && roll) ? C_FillRoll : C_FillColor), (isDown) ? C_BorderActive : C_BorderColor);
            else
                kDraw.FillRect(bounds, 2, (isDown) ? C_FillActive : ((inBounds && roll) ? C_FillRoll : C_FillColor), (inBounds && roll || isDown) ? C_BorderActive : C_BorderColor2, C_BorderColor);


        }
        private static void RenderText(Rect bounds, bool isDown, bool inBounds, bool roll)
        {
            if (SINGLE_LINE)
                kDraw.FillRect(bounds, 1, C_TxtField, (inBounds) ? C_BorderActive : C_BorderColor);
            else
                kDraw.FillRect(bounds, 2, C_TxtField, (inBounds) ? C_BorderActive : C_BorderColor2, C_BorderColor);
            /*
            if (SINGLE_LINE)
                kDraw.FillRect(bounds, 1, (isDown) ? C_FillActive : ((inBounds && roll) ? C_FillRoll : C_FillColor), (isDown) ? C_BorderActive : C_BorderColor);
            else
                kDraw.FillRect(bounds, 2, (isDown) ? C_FillActive : ((inBounds && roll) ? C_FillRoll : C_FillColor), (inBounds && roll || isDown) ? C_BorderActive : C_BorderColor2, C_BorderColor);
            */
        }
        #endregion
        //-----------------------------------------------------------------------------
        //
        // 																	Label Field
        //
        //-----------------------------------------------------------------------------
        #region Label Field

		/** LabelField. Without any keyboard or mouse activity. Only display the label caption with element layout.
		*
		* @param Rect   bounds - The position and size of this element.
		* @param string caption - The label text to display.
		*
		* */
        public static void kLabel(Rect bounds, string caption, bool roll = false)
        {
            kLabel(bounds, new GUIContent(caption), roll);
        }
        /** LabelField. Without any keyboard or mouse activity. Only display the label caption with element layout.
        *
        * @param Rect 	  bounds - The position and size of this element.
        * @param GUIContent content - A GUIContent for display a label object that can contain text, tooltip or a image.
        *
        * */
        public static void kLabel(Rect bounds, GUIContent content, bool roll = false)
        {
            int controlID = GUIUtility.GetControlID(bounds.GetHashCode(), FocusType.Native);
            bool inBounds  = bounds.Contains(Event.current.mousePosition) && roll;
   
            switch (Event.current.GetTypeForControl(controlID))
            {

                case EventType.repaint:
                    if (SINGLE_LINE)
                        kDraw.FillRect(bounds, 1, C_TxtField, (inBounds) ? C_BorderActive : C_BorderColor);
                    else
                        kDraw.FillRect(bounds, 2, C_TxtField, (inBounds) ? C_BorderActive : C_BorderColor2, C_BorderColor);
				
		           	textStyle.Draw(bounds, content, controlID);

	            break;
            }
        }


        #endregion
        //-----------------------------------------------------------------------------
        //
        // 																	TextField
        //
        //-----------------------------------------------------------------------------
        #region Text Field


		/** Input TextField. With labelfield infront of the element.
		*
		* @param Rect   bounds - The position and size of this element.
		* @param string caption - The label text to display.
		* @param string label   - The labelfield infront of the element.
		*
		* @returns string		  - The displayed textfield string.*/
		public static string kTxt( Rect bounds, string caption, GUIContent label )
        {
			DrawLabel( bounds, label );

			return kTxt( new Rect( bounds.x + LABEL_WIDTH, bounds.y, bounds.width, bounds.height ), caption );
		}
		/** Input TextField.
		*
		* Click inside to set the keyboard focus to this element.
		* In the active mode a color border will show arround the element.
		* Press RETURN or click outside the element to deactivate the textfield focus.
		*
		* @param Rect   bounds - The position and size of this element.
		* @param string caption - The label text to display.
		*
		* @returns string		  - The displayed textfield string.*/
        public static string kTxt(Rect bounds, string caption)
        {
            int controlID  = GUIUtility.GetControlID(bounds.GetHashCode(), FocusType.Passive, bounds);//bounds.GetHashCode(), FocusType.Native,);
            int keyboardID = GUIUtility.keyboardControl;
            //	bool hotControl  = GUIUtility.hotControl == controlID;
            bool hotKeyboard = controlID + 1 == keyboardID && keyboardID != 0;
            bool inBounds = bounds.Contains(Event.current.mousePosition);

            switch (Event.current.GetTypeForControl(controlID))
            {

                case EventType.repaint:
					/*if(SINGLE_LINE) 
						kDraw.FillRect(bounds, 1, C_TxtField, (inBounds || hotKeyboard) ? C_BorderActive : C_BorderColor);
					else
	                    kDraw.FillRect(bounds, 2, C_TxtField, (inBounds || hotKeyboard) ? C_BorderActive : C_BorderColor, C_BorderColor2);*/
                    RenderText(bounds, false, (inBounds || hotKeyboard), false);
                    break;

                case EventType.mouseUp:
		
			//	Debug.Log (controlID + " " + keyboardID + " " + GUIUtility.hotControl + " "  + GUIUtility.keyboardControl);
                    if (hotKeyboard) SetHotControl(false, 0, 0);

                    break;

            }
			// if (Event.current.type == EventType.mouseUp) SetHotControl(false, 0, 0);
			return EditorGUI.TextField(new Rect( bounds.x+((SINGLE_LINE)?1:2), bounds.y+((SINGLE_LINE)?1:2), bounds.width-4, bounds.height-4), caption, textStyle);
        }
        #endregion
        //-----------------------------------------------------------------------------
        //
        //  																DRAW ELEMENT
        //
        //-----------------------------------------------------------------------------


        //-----------------------------------------------------------------------------
        //
        // 																	Control ID
        //
        //-----------------------------------------------------------------------------
        #region GUI CONTROL ID
        /** Setup the Unity GUI System control id.*/
        private static bool SetHotControl(bool inBounds, int controlID, int keyboardID = 0)
        {

            if (inBounds)
            {

                GUIUtility.hotControl = controlID;
                GUIUtility.keyboardControl = keyboardID;

            }else{
				
                if (GUIUtility.hotControl == controlID) GUIUtility.hotControl = GUIUtility.keyboardControl = 0; //Debug.Log ("this control is hot !!");}
            }

            // ignore mouse while some other control has it -> AngryAnt GitHub
            if (GUIUtility.hotControl != controlID) { inBounds = false; }

            return inBounds;
        }
        # endregion
        //-----------------------------------------------------------------------------
        //
        // 																	GUIStyles
        //
        //-----------------------------------------------------------------------------
        # region Layout
		/**
		* Draw a label infront of the element.
		*
		* @param Rect bounds - The position and size of the labelfield.
		* @param GUIContent label - A GUIContent for display a label. Can contain text or a image.*/
		private static void DrawLabel( Rect bounds , GUIContent label, GUIStyle style = null )
		{
            GUIStyle _style = (style != null) ? style : labelStyle;
            if (Event.current.type == EventType.repaint) _style.Draw(new Rect(bounds.x, bounds.y, LABEL_WIDTH, bounds.height), label, 0);
		}

        /** Style for TextFields.*/
        public static GUIStyle textStyle
        {
            get
            {
                GUIStyle customSkin = new GUIStyle();
                customSkin.alignment = TextAnchor.MiddleCenter;
                customSkin.fontSize = TF_SIZE_text;

                customSkin.normal.textColor = (GUI.skin.name == "DarkSkin" ? Color.white : Color.black);
                customSkin.normal.background = null;

                return customSkin;
            }
        }
		/** Style for LabelFields infront of an element.*/
		public static GUIStyle labelStyle
        {
            get
            {
                GUIStyle customSkin = new GUIStyle();
                customSkin.alignment = TextAnchor.MiddleLeft;
                customSkin.fontSize = TF_SIZE_label;
			//	customSkin.fontStyle = FontStyle.Bold;
				customSkin.normal.textColor = C_LabField;
                return customSkin;
            }

		}		
		/** Style for List elements.*/
		public static GUIStyle listStyle
        {
            get
            {
				GUIStyle customSkin  = new GUIStyle();
	            customSkin.alignment = TextAnchor.MiddleLeft;
	            customSkin.fontSize = TF_SIZE_list;
	            customSkin.padding.left = customSkin.padding.right = customSkin.padding.top = customSkin.padding.bottom = 4;
				
				customSkin.normal.textColor = Color.black;
	            customSkin.normal.background = null;
				
				customSkin.hover.textColor = Color.white;
	            customSkin.hover.background = kGFXTexture.ColoredTexture( 4, 4, new Color( 0, .4f, .8f ) );
			
				customSkin.contentOffset = new Vector2( 10, 0 );

                return customSkin;
            }

		}
        //-----------------------------------------------------------------------------
        //
        // 																	Colors
        //
        //-----------------------------------------------------------------------------
        
        /** Returns a Unity Color object from the give RGB values.
         * @param float r 	 The Red component of the color.
         * @param float g	 The Green component of the color.
         * @param float b	 The Blue component of the color.
         * @param float a	 The Alpha component of the color.
         * 
         * @return Color     A new Color with given r,g,b,a components.*/
        public static Color RGB(float r, float g, float b, float a = 255) { float q = 1 * 255; return new Color(q * r, q * g, q * b, q * a); }

		public static void SetActColor( Color active )
		{
			C_BorderActive = active; C_BorderActive.a = .4F;	
			C_FillActive   = active; C_FillActive.a   = .3F;
			C_FillRoll     = active; C_FillRoll.a     = .2F;
		}
		public static void ResetColors()
		{
            C_BorderActive = new Color(0.1255f, .302f, .427f);
	        C_BorderColor2  = ( GUI.skin.name == "DarkSkin" ? new Color(.25f, .25f, .25f): new Color(.55f, .55f, .55f));
	        C_BorderColor = ( GUI.skin.name == "DarkSkin" ? new Color (.19f, .19f, .19f)  : Color.grey); 
	        C_FillActive   = new Color(   0, .592f, .984f);
	        C_FillRoll 	   = new Color(   0.1255f, .302f, .427f );
	        C_FillColor    = ( GUI.skin.name == "DarkSkin" ? new Color(.35f, .35f, .35f): new Color(.65f, .65f, .65f));
	        C_TxtField 	   = ( GUI.skin.name == "DarkSkin" ? RGB( 0,66,212 ): new Color(.85f, .85f, .85f));
            //new Color(.65f, .65f, .65f)
	/*		C_BorderActive = new Color(   0, .4f, .8f, 1f);
	        C_BorderColor  = ( GUI.skin.name == "DarkSkin" ? new Color(.35f, .35f, .35f): new Color(.55f, .55f, .55f));
	        C_BorderColor2 = ( GUI.skin.name == "DarkSkin" ? new Color (.3f, .3f, .3f)  : Color.grey); 
	        C_FillActive   = new Color(   0, .4f, .8f, .8f);
	        C_FillRoll 	   = new Color(   0, .4f, .8f, .2f);
	        C_FillColor    = ( GUI.skin.name == "DarkSkin" ? new Color(.45f, .45f, .45f): new Color(.65f, .65f, .65f));
	        C_TxtField 	   = ( GUI.skin.name == "DarkSkin" ? new Color(.25f, .25f, .25f): new Color(.85f, .85f, .85f));*/
		}
		public static void ResetTFSize()
		{
			TF_SIZE_text  = 11;
			TF_SIZE_label = 11;
			TF_SIZE_list  = 11;
		}
		
		
        /** Color for the active element border.*/
        public static Color C_BorderActive = new Color(0, .4f, .8f, 1f);
        /** Color for the element border.*/
        public static Color C_BorderColor  = (GUI.skin.name == "DarkSkin" ? new Color(.25f, .25f, .25f): new Color(.55f, .55f, .55f));
        /** Color for the second element border.*/
        public static Color C_BorderColor2 = (GUI.skin.name == "DarkSkin" ? new Color (.3f, .3f, .3f)  : Color.grey); 

        /** Color for the active element fillin.*/
        public static Color C_FillActive = new Color(0, .4f, .8f, .8f);
        /** Color for the over element fillin.*/
        public static Color C_FillRoll 	 = new Color(0, .4f, .8f, .2f);
        /** Color for the element fillin.*/
        public static Color C_FillColor = (GUI.skin.name == "DarkSkin" ? new Color(.45f, .45f, .45f): new Color(.65f, .65f, .65f));
        /** Color for the textField fillin.*/
        public static Color C_TxtField 	= (GUI.skin.name == "DarkSkin" ? new Color(.25f, .25f, .25f):new Color(.85f, .85f, .85f));
		 /** Color for the labelField fillin.*/
        public static Color C_LabField 	= (GUI.skin.name == "DarkSkin" ? new Color(.62f, .62f, .62f):new Color(.28f, .28f, .28f));
		/** Draw Mode. Layout option to set a single or double outline.*/
		public static bool SINGLE_LINE = false;
		/** The space for the label field in front of a guiElement.*/
		public static float LABEL_WIDTH  = 40;
		/** Button width for ComboBox and Popup Components.*/
		public static float BTN_WIDTH = 16;
		/** Text size for - Textfield elements.*/
		public static int TF_SIZE_text  = 11;
		/** Text size for - Labelfield elements.*/
		public static int TF_SIZE_label = 11;
		/** Text size for - List elements.*/
		public static int TF_SIZE_list  = 11;
		
        #endregion
        //-----------------------------------------------------------------------------
        //
        // 																	Helpers
        //
        //-----------------------------------------------------------------------------
        #region Helper
        /** Check a float value if it in a range of min and max.
         *  if (value > max) value = max
         *  if (value < min) value = min
         *
         *  @param float  value - The value to check.
         *  @param float  min	- The minimum number.
         *  @param float  max	- The maximum number.
         *
         *  @returns float 		- The corrected value. */
        protected static float SetRange(float _value, float _min, float _max)
        {
            if (_value > _max) _value = _max;
            if (_value < _min) _value = _min;

            return _value;
        }
        #endregion

    }
}