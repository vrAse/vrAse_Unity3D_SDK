/*
 *                                 _____ _____  _  __
 *               /\               / ____|  __ \| |/ /
 *  __   ___ __ /  \   ___  ___  | (___ | |  | | ' / 
 *  \ \ / / '__/ /\ \ / __|/ _ \  \___ \| |  | |  <  
 *   \ V /| | / ____ \\__ \  __/  ____) | |__| | . \ 
 *    \_/ |_|/_/    \_\___/\___| |_____/|_____/|_|\_\ 
 *
 *
 * by Nestor Viña Leon 
 * 
 * vrAseSDK is a kit for Unity3D developers with tools to easily produce 
 * amazing VR apps and games with vrAse.
 * 
 * Copyright(c) vrAse and Nestor Viña Leon 2014
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 * 
 */

/* The vrAse camera sensor lets you use the camera as an extra button,
 * hidding it with your hand.
 * 
 * To know if the hand is hidding the camera or not simply drop this
 * script in any static object of the scene and acces the boolean value
 * doing something like this:
 * 
 * ...
 * 	void Update(){
 * 		if(VraseCameraSensor.detected){
 * 			...
 * 		}
 * }
 * ...
 * 
 * Note: We are improving the performance of the script, but if you want 
 * you can collaborate in github, this is free software.
 */

using UnityEngine;
using System.Collections;

public class VraseCameraSensor : MonoBehaviour {

	public float threshold = 0.01f;

	public static bool detected;

	private WebCamTexture cameraInput;
	private Color[] colors;

	void Start () {
		cameraInput = new WebCamTexture(5,5);
		cameraInput.anisoLevel = 0;
		cameraInput.Play();
	}
	
	// Update is called once per frame
	void Update () {
		detected = false;
		colors = cameraInput.GetPixels();
		float average = 0.0f;
		for( int i = 0; i < colors.Length; i++)
			average += approximatedGrey(colors[i]);

		average /= colors.Length;

		if( average <= threshold)
			detected = true;
	}
	
	float approximatedGrey(Color col){
		return (col.r + col.g + col.b)/3;
	}
}
