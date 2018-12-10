using System.Collections.Generic;
using UnityEngine;

namespace Helpers
{
  public class ResourcesCache
  {
    private readonly Dictionary<string, Texture2D> _texturesCache = new Dictionary<string, Texture2D>();

    public Texture2D LoadTexture(string texture)
    {
      if (_texturesCache.ContainsKey(texture))
      {
        return _texturesCache[texture];
      }

      return _texturesCache[texture] = Resources.Load<Texture2D>(texture);
    }
  }
}