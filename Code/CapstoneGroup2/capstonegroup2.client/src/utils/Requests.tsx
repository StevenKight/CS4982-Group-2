/**
 * @file Requests.tsx
 * @description: This file contains all the requests to the server
 * @module: Utils
 * @version: 1.0
 * @since: 02/02/2024
 * @author: Steven Kight
 */
class Requests {

    /**
	 * Makes a get request data to the given url and returns response as json.
	 * 
	 * @async @static
	 * @param {string} url The url to make the get request to.
     * @param {object} headers The headers to send with the get request.
	 * @returns response data as json
	 * @throws error if request fails
	 * @example
	 * const data = await Requests.get('https://example.com');
	 * console.log(data);
	 * @example
	 * Requests.get('https://example.com')
	 * 	.then(data => console.log(data))
	 * 	.catch(error => console.log(error));
	 * @memberof Requests
	 */
	static async get(url: string, headers: object) {
		try {
			const response = await fetch(url, {
                method: 'GET',
                headers: {
                    Accept: 'application/json',
                    'Content-Type': 'application/json',
                    ...headers
                }
            
            });
			const data = await response.json();
			return data;
		}
		catch (error: any) {
			throw Error(error);
		}
	}

	/**
	 * Makes a get request to the given url and returns the true if accepted and false otherwise.
	 * 
	 * @async @static
	 * @param {string} url The url to make the get request to.
     * @param {object} headers The headers to send with the get request.
	 * @returns true if accepted and false otherwise
	 * @throws error if request fails
	 * @example
	 * const data = await Requests.get('https://example.com');
	 * console.log(data);
	 * @example
	 * Requests.get('https://example.com')
	 * 	.then(data => console.log(data))
	 * 	.catch(error => console.log(error));
	 * @memberof Requests
	 */
	static async getAcceptable(url: string, headers: object) {
		try {
			const response = await fetch(url, {
                method: 'GET',
                headers: {
                    Accept: 'application/json',
                    'Content-Type': 'application/json',
                    ...headers
                }
            });
			return response.status === 202;
		}
		catch (error: any) {
			return false;
		}
	}

	/**
	 * Makes a post request to the given url with the given body(json) and returns response as json.
	 * 
	 * @async @static
	 * @param {string} url The url to make the post request to.
	 * @param {object} body The body to send with the post request.
     * @param {object} headers The headers to send with the post request.
	 * @returns response data as json
	 * @throws error if request fails
	 * @example
	 * const data = await Requests.postJson('https://example.com', {});
	 * console.log(data);
	 * @example
	 * Requests.postJson('https://example.com', {})
	 * 	.then(data => console.log(data))
	 * 	.catch(error => console.log(error));
	 * @memberof Requests
	 * @see https://developer.mozilla.org/en-US/docs/Web/API/Body/json
	 */
	static async postJson(url: string, headers: object, body: object) {
		try {
			const response = await fetch(url, {
				method: 'POST',
				body: JSON.stringify(body),
				headers: {
					Accept: 'application/json',
					'Content-Type': 'application/json',
                    ...headers
				}
			});
			
			try {
				const data = await response.json();
				return data;
			}
			catch (error) {
				return response;
			}
		}
		catch (error: any) {
			throw Error(error);
		}
	}

    static async login(username: string, password: string) {
        var body = { username: username, password: password };
        
        var data = await Requests.postJson('login', body, {});
        localStorage.setItem('token', data.token);
        return data;
    }

    static async createUser() {
        var url = 'https://localhost:7041/users';
    }

    static async getNotes() {
        var url = 'https://localhost:7041/note';
        return await Requests.get(url, {
            'Authorization': 'Bearer ' + localStorage.getItem('token')
        });
    }

    static async getNote(sourceId: number) {
        var url = 'https://localhost:7041/note/' + sourceId;
        return await Requests.get(url, {
            'Authorization': 'Bearer ' + localStorage.getItem('token')
        });
    }

    static async createNote() {
        var url = 'https://localhost:7041/note';
    }

    static async updateNote() {
        var url = 'https://localhost:7041/note';
    }

    static async deleteNote() {
        var url = 'https://localhost:7041/note';
    }

    static async getSources() {
        var url = 'https://localhost:7041/source';
        return await Requests.get(url, {
            'Authorization': 'Bearer ' + localStorage.getItem('token')
        });
    }

    static async getSource(sourceId: number) {
        var url = 'https://localhost:7041/source/' + sourceId;
        return await Requests.get(url, {
            'Authorization': 'Bearer ' + localStorage.getItem('token')
        });
    }

    static async createSource() {
        var url = 'https://localhost:7041/source';
    }

    static async updateSource() {
        var url = 'https://localhost:7041/source';
    }

    static async deleteSource() {
        var url = 'https://localhost:7041/source';
    }

    static async sharedNotes() {
        var url = 'https://localhost:7041/shared';
        return await Requests.get(url, {
            'Authorization': 'Bearer ' + localStorage.getItem('token')
        });
    }

    static async sharedNote(sourceId: number, username: string) {
        var url = 'https://localhost:7041/shared/' + sourceId + '-' + username;
        return await Requests.get(url, {
            'Authorization': 'Bearer ' + localStorage.getItem('token')
        });
    }

    static async shareNote() {
        var url = 'https://localhost:7041/shared';
    }

    static async unshareNote() {
        var url = 'https://localhost:7041/shared';
    }

    static async updateSharedNote() {
        var url = 'https://localhost:7041/shared';
    }
}

export default Requests;