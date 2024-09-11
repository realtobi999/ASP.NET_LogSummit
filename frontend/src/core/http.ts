export async function makeHttpRequest(
    url: string,
    method: string,
    body?: any,
    headers: Record<string, string> = {}
): Promise<any> {
    const response = await fetch(url, {
        method,
        headers: headers,
        body: body ? JSON.stringify(body) : null,
    });

    if (!response.ok) {
        console.error(`HTTP error! Status: ${response.status}`);
    }

    const data = await response.json();
    return data;
}