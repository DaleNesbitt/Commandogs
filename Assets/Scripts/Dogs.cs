using System.Collections;
using UnityEngine;

public class Dogs : MonoBehaviour
{
    public float walkSpeed = 0.75f;
    public float runSpeed = 1.3f;
    public float rotationSpeed = 5f;
    public string destinationTag = "DestPoint";

    private Transform destPoint;
    private bool isEscaping = false;

    public void TriggerEscape()
    {
        if (isEscaping) return;  // Prevent the method from being called multiple times simultaneously
        isEscaping = true;  // Set the flag to true so other calls don't start the coroutine
        StartCoroutine(MoveAllDogsSequentially());
    }

    private IEnumerator MoveAllDogsSequentially()
    {
        // Find the destination point if it's not already set
        if (destPoint == null)
        {
            GameObject destObj = GameObject.FindGameObjectWithTag(destinationTag);
            if (destObj != null)
            {
                destPoint = destObj.transform;
            }
            else
            {
                yield break; // Stop if no destination is found
            }
        }

        // Move each dog one by one
        foreach (Transform dog in transform)
        {
            if (!dog.gameObject.activeInHierarchy) continue; // Skip inactive dogs

            // Set a random animation (walk or run)
            Animator animator = dog.GetComponent<Animator>();
            int animationID = Random.Range(3, 5); // 3 is walk, 4 is run
            if (animator != null)
            {
                animator.SetInteger("AnimationID", animationID);
            }

            // Determine speed based on animation type
            float speed = (animationID == 4) ? runSpeed : walkSpeed;

            // Rotate and move the dog towards the exit
            Vector3 direction = (destPoint.position - dog.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            // Move dog towards the exit
            while (Vector3.Distance(dog.position, destPoint.position) > 0.1f)
            {
                dog.rotation = Quaternion.Slerp(dog.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                dog.position = Vector3.MoveTowards(dog.position, destPoint.position, speed * Time.deltaTime);
                yield return null;  // Ensure we move frame-by-frame
            }

            // Disable the dog once it reaches the exit
            dog.gameObject.SetActive(false);

            // Add a small delay before the next dog starts moving
            yield return new WaitForSeconds(0.2f);  // Adjust this delay if dogs are moving too fast
        }

        isEscaping = false;  // Reset the flag when all dogs have finished moving
    }

}
