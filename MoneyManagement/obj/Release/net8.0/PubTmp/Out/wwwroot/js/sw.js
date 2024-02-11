// Activate event: Clean up old caches
// Define the cache name
const cacheName = 'your-app-cache-v1';

// List of assets to cache
const assetsToCache = [
  '/',
  '/index.html',
  '/path/to/your/css/styles.css',
  '/path/to/your/js/script.js',
  'https://via.placeholder.com/128x128?text=App+Icon',
  'https://via.placeholder.com/192x192?text=App+Icon',
  'https://via.placeholder.com/512x512?text=App+Icon',
  // Add a wildcard to match all pages (*)
  '/*'
];

// Install event: Cache the essential assets
self.addEventListener('install', (event) => {
  event.waitUntil(
    caches.open(cacheName)
      .then(cache => cache.addAll(assetsToCache))
      .then(() => self.skipWaiting())
  );
});

// Activate event: Clean up old caches
self.addEventListener('activate', (event) => {
  event.waitUntil(
    caches.keys()
      .then(cacheNames => {
        return Promise.all(
          cacheNames.filter(name => name !== cacheName)
            .map(name => caches.delete(name))
        );
      })
      .then(() => self.clients.claim())
  );
});

// Fetch event: Serve assets from cache, and update cache if necessary
self.addEventListener('fetch', (event) => {
  event.respondWith(
    caches.match(event.request)
      .then(response => {
        if (response) {
          return response;
        }

        // Clone the request because it can only be consumed once
        const fetchRequest = event.request.clone();

        return fetch(fetchRequest)
          .then(response => {
            if (!response || response.status !== 200 || response.type !== 'basic') {
              return response;
            }

            // Clone the response because it can only be consumed once
            const responseToCache = response.clone();

            caches.open(cacheName)
              .then(cache => cache.put(event.request, responseToCache));

            return response;
          });
      })
  );
});