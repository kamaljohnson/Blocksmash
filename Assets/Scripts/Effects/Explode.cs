using UnityEngine;

public class Explode : MonoBehaviour {

	public ParticleSystem DestructionEffect;
	
	
	public void explode(Material material)
	{
		ParticleSystem explosionEffect = Instantiate(DestructionEffect) 
			as ParticleSystem;
		
		explosionEffect.transform.position = transform.position;
		explosionEffect.GetComponent<Renderer>().material = material;
		explosionEffect.loop = false;
		explosionEffect.Play();
 
		Destroy(explosionEffect.gameObject, explosionEffect.duration);

		Destroy(gameObject);
     
	}
}
