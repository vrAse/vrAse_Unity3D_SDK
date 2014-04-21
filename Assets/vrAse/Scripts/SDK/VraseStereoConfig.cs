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

/* If you activate the gestures, you can change screen mode touching 
 * with 2 fingers to 2D (not SBS mode) and 3 fingers for vrAse mode. 
 */
using UnityEngine;
using System.Collections;

public class VraseStereoConfig : MonoBehaviour {

	public bool vraseEnabled = true;
	public bool gesturesActivated = false;

	public static float IPD = 0.26f;

	private static GameObject vraseLeft, vraseRight;

	void Start (){

		Screen.orientation = ScreenOrientation.LandscapeLeft;
		Screen.sleepTimeout = SleepTimeout.NeverSleep;

		vraseLeft = GameObject.Find("VraseLeft");
		vraseRight = GameObject.Find("VraseRight");

		if(vraseEnabled)
			enableVrase();
		else
			disableVrase();
	}
	

	void Update (){
		if(gesturesActivated)
			if(Input.touchCount != 0)
				if (Input.touchCount == 2)
					disableVrase();
				else if (Input.touchCount == 3)
					enableVrase();				

	}

	public static void enableVrase (){
		setIPD(IPD);
		vraseLeft.GetComponent<Camera>().rect = new Rect(0.0f,0.0f,0.5f,1.0f);
		vraseRight.GetComponent<Camera>().enabled = true;

	}

	public static void disableVrase (){
		setIPD(0.0f);
		vraseLeft.GetComponent<Camera>().rect = new Rect(0.0f,0.0f,1.0f,1.0f);
		vraseRight.GetComponent<Camera>().enabled = false;
	}

	public static void setIPD (float cameraDistance){
		IPD = cameraDistance;

		vraseLeft.transform.localPosition = new Vector3 (-cameraDistance,
		                                                                vraseLeft.transform.localPosition.y,
		                                                                vraseLeft.transform.localPosition.z);
		vraseRight.transform.localPosition = new Vector3 (cameraDistance,
		                                                                 vraseRight.transform.localPosition.y,
		                                                                 vraseRight.transform.localPosition.z);
	}

}
