/**Copyright (c) 2011 Paul Knab - Version 0.0.1*/
package
{
	import flash.display.*;
	import flash.events.*;
	import flash.geom.Rectangle;
	import flash.text.TextFieldType;
	import flash.text.engine.FontWeight;
	
	import klock.simpleComponents.components.Button;
	import klock.simpleComponents.components.CheckBox;
	import klock.simpleComponents.components.ComboBox;
	import klock.simpleComponents.components.InputText;
	import klock.simpleComponents.components.Label;
	import klock.simpleComponents.components.List;
	import klock.simpleComponents.components.ListItem;
	import klock.simpleComponents.components.NumStep;
	import klock.simpleComponents.components.Panel;
	import klock.simpleComponents.components.RadioButton;
	import klock.simpleComponents.components.ScrollBar;
	import klock.simpleComponents.components.Slider;
	import klock.simpleComponents.components.base.*;
	import klock.simpleComponents.fonts.FontManager;

	public class SimpleComponents extends Sprite
	{
		public function SimpleComponents()
		{

			var f:FontManager = new FontManager();
			
			var pb:Button    = new Button   ( {name:"BTN2", x: 50, y:9, pushMode:true }, "PushButton", this )
			var b :Button    = new Button   ( {name:"BTN1", x: 50, y:30 }, "button", this )
			var b2:Button    = new Button   ( {x:151, y:30, Toggled:true}, "Toggle Button", this )
			var i :InputText = new InputText( {x:50,  y:51, textAlign:"center"   }, "type in ...", this )
			var l :Label     = new Label    ( {x:151, y:51, textAlign:"center" }, "Label 01", this )
			var n :NumStep   = new NumStep  ( {x: 50, y:72 }, 10, this )
			var ch:CheckBox  = new CheckBox ( 		{x: 50, y:93  }, "CheckBox", 	 this, onChange )
			var rb:RadioButton = new RadioButton ( 	{x: 50, y:114 }, "Radio Button", this ,onChange )
					
			var p1:Panel	= new Panel		( {x: 50, y:145}, this )
				p1.showGrid = true
			//	p1.gridColor = 0xff6600
			//	p1.gridSize = 20
			//	p1.color = 0xeaeaeae
				p1.alpha = .6
			
			var n2 :NumStep = new NumStep  ({x:5, y:5  }, 10, p1.content )
				n2.setSize( 60, 15)
				n2.Enabled = false
						
			var n3 :NumStep = new NumStep ({x:5, y:20  }, 10, p1.content )
				n3.setSize( 60, 15)
			
			var vs:Slider = new Slider( {name:"VSlider", x:263,y:30}, 				this, Slider.VERTICAL, 		onChange)
				vs.setSliderParams( 0, 2, 1 )
			
			var hs:Slider = new Slider( {name:"HSlider", x: 252,y:4 }, 				this, Slider.HORIZONTAL,	onChange)
				hs.setSliderParams( 0, 10, 0 )
			
			var sb:ScrollBar = new ScrollBar(  {name:"ScrollBar", x: 252, y:15 }, 	this, Slider.HORIZONTAL, 	onChange)
				sb.setSliderParams( 0 , 5, 5 )
			
			var vsb:ScrollBar = new ScrollBar( {name:"VScrollBar",x:252, y:30 }, 	this, Slider.VERTICAL, 		onChange)
				vsb.setSliderParams( 0 , 5, 0 )
					
			var li:List = new List( {x: 274, y: 30 },this , ["ich","du","er","sie","es","wir","ihr","sie"])
				
			
			var cb:ComboBox = new ComboBox( {name:"Combo",x: 380, y: 30 },"Select person", this, ["ich","du","er","sie","es","wir","ihr","sie"], null)
				cb.addEventListener( Event.SELECT, onChange )
		
			//trace( "------------------------------")

		}
		
		private function onChange( event:Event ):void{
	//		trace( event, event.target )
			if( event.target is RadioButton )	{ 	trace( RadioButton( event.target ).selected ); 	}
			if( event.target is ComboBox )		{ 	trace( ComboBox( event.target ).selectedIndex); }
			if( event.target is CheckBox )		{	trace( CheckBox( event.target ).selected ) 		}
	//		if( event.target == ScrollBar )	{ trace( event.target.name, ScrollBar(event.target ).value ); return ;}
		}
	}
}