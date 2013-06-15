/* 

kTile Anim  V.0.1 - 2013 - Paul Knab 
____________________________________
  
Extended TileBase Extension 
Description : Animation/Sprite Sheets/ TextureAtlas

Updates: 
------------------------------------
 14-06-2013
  
 recreate class structure
 
*/
using UnityEngine;
using System.Collections;

public class TileAnim : TileBase
{
    public int _currentFrame = 0;
    public int _lastFrame = 0;
    public float _frameTick = 0.1f;
    public Rect[] _frameRects;
    private float _frameTime = 0.0f;
    private int _frameTemp = 0;

    //[HideInInspector]
    public bool playAnimation = false;
    //[HideInInspector]
    public bool looping = false;
    //	[HideInInspector]
    //	public 		bool 		reverse	= false;

    //#if UNITY_EDITOR

    protected override void Update()
    {
        if (playAnimation && Application.isPlaying)
            Frame_check();
        if (facing == FACING.BB)
            FaceDirection();
    }

    //#endif
    /** Helper method to the current uvRect object. 
     * 	@params returns Rect - The Rectangle object of uvRect.*/
    protected override Rect UV_rect()
    {
        uvRect = FrameRect;

        _width = (uvRect.width / 100);
        _height = (uvRect.height / 100);

        return uvRect;
    }
    // ---------------------------------------------- Animation Frame Setup
    /** Main animation call.*/
    private void Frame_check()
    {
        _frameTemp = _currentFrame;

        Frame_calc();

        //		Debug.Log ("-> "+( _frameTemp != _currentFrame ));// + " - " + i++);

        if (_frameTemp != _currentFrame)
        {

            Frame_draw();

            //i=0;
        }
    }
    /** Main animation frame draw call.*/
    private void Frame_draw()
    {
        if (_currentFrame < 0)
            return;

        MESH_setup(FrameRect);
    }
    /** Calculate the Animation frame.*/
    private void Frame_calc()
    {

        if (playAnimation)
        {
            if (_frameTime < 1.0f)
            {
                _frameTime += _frameTick / 10;
            }
            else
            {
                if (_currentFrame == _lastFrame)
                {
                    // Animation Ends
                    if (looping)
                    {
                        _currentFrame = -1;
                    }
                    else
                    {
                        // stop animation
                        playAnimation = false;
                    }
                    return;
                }
                else
                {
                    // Frame reached Step 2 the Next 
                    _frameTime = 0.0f;

                    /*if( reverse )
                    {
                        _currentFrame = ( _currentFrame > 0 ) ? --_currentFrame :  0;
						
                        if( _currentFrame == 0 ) reverse = false;	
		
                    }else{
                    */
                    _currentFrame = (_currentFrame < _lastFrame) ? ++_currentFrame : _lastFrame;

                    //}
                    if (_currentFrame < 0)
                        _currentFrame = 0;
                    if (_currentFrame > _lastFrame)
                        _currentFrame = _lastFrame;
                }
            }
        }
    }
    /** Reset the Animation frame time to the value of zero.*/
    public void ResetFrameTime()
    {
        _frameTime = 0f;
    }
    /** Get the actual uv Frame from the sprite animation rect List. 
     * @returns Rect r - The rectangle object.*/
    public Rect FrameRect
    {
        get
        {
            int f = (_currentFrame > _frameRects.Length ? _lastFrame : _currentFrame < 0 ? 0 : _currentFrame);
            Rect r = _frameRects[f];
            return r;
        }
    }
    /** Set the sprite animation frames Rect List. 
     * @params Rect[] value - The uv rects for the sprite animation.*/
    public Rect[] FrameRects
    {
        set { _frameRects = value; }
    }
}