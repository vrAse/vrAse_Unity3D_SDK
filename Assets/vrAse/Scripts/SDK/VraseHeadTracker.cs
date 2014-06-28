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

using UnityEngine;
using System.Collections;

public class VraseHeadTracker : MonoBehaviour {
	
	public bool exact = false;
	
	Gyroscope gyro;	
	Quaternion rotation;
	float multiplier;
	
	void Start () {
		
		gyro = Input.gyro;
		gyro.enabled = true;
		gyro.updateInterval = 0.001f;
		
	}
	
	void Update () {
		if( Time.frameCount < 30 || exact)
			ExactMode();
		else
			SmoothMode();
		
	}
	
	//This mode tries to be exact, it use the returned device orientation from Unity API
	void ExactMode(){
		rotation = gyro.attitude;
		rotation *= new Quaternion(0,0,1,0);		
		transform.localRotation = rotation;
	}
	
	//Mode that tries to be precise in a smoothed way but less fluent, used to correct the accumulated error
	void PreciseMode(){
		
		rotation = gyro.attitude;
		rotation *= new Quaternion(0,0,1,0);
		
		multiplier = Quaternion.Angle(transform.localRotation, rotation );
		
		transform.localRotation = Quaternion.RotateTowards(transform.localRotation, 	
		                                                   rotation,
		                                                   Time.deltaTime * multiplier / 2) ;
	}
	
	//Main mode focused on achieve a realistic and fluid movement feeling
	void SmoothMode(){
		
		transform.RotateAround(transform.position,
		                       transform.up,
		                       -gyro.rotationRate.y * Mathf.Rad2Deg * Time.deltaTime);
		transform.RotateAround(transform.position,
		                       transform.forward,
		                       gyro.rotationRate.z * Mathf.Rad2Deg * Time.deltaTime);
		transform.RotateAround(transform.position,
		                       transform.right,
		                       -gyro.rotationRate.x * Mathf.Rad2Deg * Time.deltaTime);
		
		if(!IsQuickRotation() && IsBigError())
			PreciseMode();
		
	}
	
	//Determines if the movement is being fast or slow
	bool IsQuickRotation(){
		if(Vector3.Magnitude(gyro.rotationRate*Time.deltaTime)>0.1f)
			return true;
		else
			return false;		

	}
	
	//Returns if accumulated error is bigger than the threshold
	bool IsBigError(){
		return ( Quaternion.Angle(gyro.attitude*(new Quaternion(0,0,1,0)), transform.localRotation) > 4.0f);
	}
}
