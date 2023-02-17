using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityRandom = UnityEngine.Random;

namespace ProjectDion._Scripts
{
	[CreateAssetMenu(menuName = "Weapons/GunSO", fileName = "Gun")]
	public class GunSO : ScriptableObject
	{
		//Properties

		[Header("Physical/Visual Properties")]
		[SerializeField] private GameObject _weaponGO;
		[SerializeField] private GameObject _bulletGO;
		[SerializeField] private GameObject _muzzleFlash;

		[Header("Individual Attributes")]
		[SerializeField] private float _damage;
		[SerializeField] private int _bulletsPerMinute;
		[SerializeField] private int _magazineSize;
		[SerializeField] private float _reloadTime;
		[SerializeField] private float _critDamageMultiplier = 2;
		[SerializeField] [Range(0, 1)] private float _weaponWeight;
		[SerializeField] [Range(-1,1)] private float _weaponSights;

		[Header("RunTime Properties")]
		[SerializeField] private int _bulletsInMag;

		[Header("Hand Placement")]
		[SerializeField] private Vector3 _positionInHand;
		[SerializeField] private Quaternion _rotationInHand;

		[Header("Hidden Variables")]

		public bool canShoot = true;
		

		//Methods

		public virtual void PlaceInHand(Transform parentBone)
        {
			GameObject weapon = Instantiate(_weaponGO);
			weapon.transform.parent = parentBone;
			weapon.transform.localPosition = _positionInHand;
			weapon.transform.localRotation = _rotationInHand;
        }
		public virtual void Shoot()
        {
        }
		public virtual void Reload()
        {

        }


	}
}