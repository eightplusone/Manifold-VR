﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Duplicator : MonoBehaviour
{
    public GameObject target;
    private int clonecount = 0;
	private int clonesPerSide = 3;

    void Start()
    {
        if (target != null)
        {
            GameObject clonegroup = new GameObject(target.name + "-clones");
            clonegroup.transform.position = new Vector3(0, 0, 0);
            clonegroup.transform.localScale = new Vector3(1, 1, 1);
            clonegroup.transform.localRotation = new Quaternion();

			clonesPerSide = 3;
			for (int i = -clonesPerSide; i <= clonesPerSide; i++)
				for (int j = -clonesPerSide; j <= clonesPerSide; j++)
					for (int k = -clonesPerSide; k <= clonesPerSide; k++)
                    {
                        if (i == 0 && j == 0 && k == 0)
                            continue;
                        float sx = ((i + j + k) % 2 == 0 ? 1.0f : -1.0f);
                        GameObject clone = TransformedClone(
                            new Vector3(3f * i, 3f * j, 3f * k),
                            new Vector3(sx, 1, 1),
                            new Quaternion()
                        );
                        clone.transform.parent = clonegroup.transform;
                    }
        }
    }

    private GameObject TransformedClone(Vector3 translation, Vector3 scale, Quaternion rotation)
    {
        // Make a full hierarchical clone of the input object and all components
        GameObject clone = Instantiate(target);
        clone.name = target.name + "-clone" + clonecount;

        // Disable all scripts on the clone
        // based on http://answers.unity3d.com/questions/292802
        MonoBehaviour[] scripts = clone.GetComponents<MonoBehaviour>();
        foreach (MonoBehaviour script in scripts)
        {
            Destroy(script);
        }

        // Attach the TransformFollow script
        TransformFollower tf = clone.AddComponent<TransformFollower>() as TransformFollower;
        tf.translation = translation;
        tf.scale = scale;
        tf.rotation = rotation;
        tf.leader = target;

        clonecount++;
        return clone;
    }
}
