using UnityEngine;
using System.Collections;

public class UVFrameAnimation : MonoBehaviour
{
	[SerializeField]
	int _tileX = 4;

	[SerializeField]
	int _tileY = 4;

	[SerializeField]
	float _framesPerSecond = 10f;

	[SerializeField]
	bool _isLoop;

	Renderer _meshRenderer;

	float _startTime;

	public int currentFrame;

	void Awake()
	{
		_meshRenderer = GetComponent<Renderer>();
		_startTime = Time.time;
	}

	void OnEnable()
	{
		_startTime = Time.time;
	}

	void Update()
	{
		int numFrames = _tileX * _tileY;
		currentFrame = Mathf.FloorToInt((Time.time - _startTime) * _framesPerSecond);
		if(!_isLoop)
		{
			currentFrame = Mathf.Min(currentFrame, numFrames-1);
		}

		float tileXF = (float) _tileX;
		float tileYF = (float) _tileY;

		float uvY = (float) (currentFrame / _tileX);
		float uvx = (float) (currentFrame % _tileX);

		Vector2 size = new Vector2(1f/tileXF, 1f/tileYF);

		Vector2 offset = new Vector2(uvx/(tileXF), (tileYF-1f-uvY)/tileYF);

		_meshRenderer.material.SetTextureOffset ("_MainTex", offset);
		_meshRenderer.material.SetTextureScale ("_MainTex", size);
	}
}
