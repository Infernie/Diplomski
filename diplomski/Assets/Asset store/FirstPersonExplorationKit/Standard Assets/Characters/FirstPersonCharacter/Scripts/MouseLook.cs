using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets.Characters.FirstPerson {

	public class MouseLook : MonoBehaviour {

	    public float XSensitivity = 2f;
	    public float YSensitivity = 2f;
	    public bool clampVerticalRotation = true;
	    public float MinimumX = -90.0f;
	    public float MaximumX = 90.0f;
	    public bool smooth;
	    public float smoothTime = 5f;


	    private Quaternion m_CharacterTargetRot;
	    private Quaternion m_CameraTargetRot;

		[Header("Custom Flags")]
		[Tooltip("Toggle mouse look on and off")]
		public bool enableMouseLook = true;
		[Tooltip("Flip Y Axis")]
		public bool flipGamepadY = true;


	    public void Init(Transform character, Transform camera){

	        m_CharacterTargetRot = character.localRotation;
	        m_CameraTargetRot = camera.localRotation;

	    }

	    public void LookRotation(Transform character, Transform camera){

			if(enableMouseLook){

				float yRot = CrossPlatformInputManager.GetAxis("Mouse X")*XSensitivity;
				float xRot = CrossPlatformInputManager.GetAxis("Mouse Y")*YSensitivity;

				// If there was no mouse input, use gamepad input instead
				if(xRot == 0.0f && yRot == 0.0f){

					yRot = CrossPlatformInputManager.GetAxis("Gamepad Look X")*XSensitivity;
					xRot = CrossPlatformInputManager.GetAxis("Gamepad Look Y")*YSensitivity;
					if(flipGamepadY){
						xRot *= -1;
					}

				}

				m_CharacterTargetRot *= Quaternion.Euler(0.0f, yRot, 0.0f);
				m_CameraTargetRot *= Quaternion.Euler (-xRot, 0.0f, 0.0f);

				if(clampVerticalRotation){
					m_CameraTargetRot = ClampRotationAroundXAxis (m_CameraTargetRot);
				}

				if(smooth){
					character.localRotation = Quaternion.Slerp (character.localRotation, m_CharacterTargetRot, smoothTime * Time.deltaTime);
					camera.localRotation = Quaternion.Slerp (camera.localRotation, m_CameraTargetRot,smoothTime * Time.deltaTime);
				}else{
					character.localRotation = m_CharacterTargetRot;
					camera.localRotation = m_CameraTargetRot;
				}

			}

	    }


	    Quaternion ClampRotationAroundXAxis(Quaternion q){

	        q.x /= q.w;
	        q.y /= q.w;
	        q.z /= q.w;
	        q.w = 1.0f;

	        float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan (q.x);
	        angleX = Mathf.Clamp (angleX, MinimumX, MaximumX);
	        q.x = Mathf.Tan (0.5f * Mathf.Deg2Rad * angleX);

	        return q;

	    }

	}

}
