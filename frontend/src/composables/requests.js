export async function makeHttpRequest(method, url, body = null, headers = {}) {
    console.log("Making request...");

    // Build the fetch options object
    const options = {
        method: method,
        headers: headers,
    };

    // Only include body if it's a POST, PUT, or PATCH request
    if (method === 'POST' || method === 'PUT' || method === 'PATCH') {
        options.body = JSON.stringify(body);
    }

    const response = await fetch(url, options);
    const data = await response.json();

    if (!response.ok) {
        console.error(`HTTP error! Status: ${response.status}`);
    }
    
    console.log("Successful request!");
    return data;
}
