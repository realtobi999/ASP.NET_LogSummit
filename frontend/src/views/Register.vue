<template>
    <div class="shadow bg-zinc-800 px-3 py-5 rounded-xl max-w-[500px]">
        <h1 class="text-4xl font-medium hover:text-purple-500 transition">Register Form</h1>
        <form class="mt-3">
            <input 
                type="text" 
                v-model="formData.username" 
                class="bg-zinc-900 mt-2 w-full p-2 rounded-lg" 
                placeholder="Enter your username" 
            />
            <input 
                type="email" 
                v-model="formData.email" 
                class="bg-zinc-900 mt-2 w-full p-2 rounded-lg" 
                placeholder="Enter your email" 
            />
            <input 
                type="password" 
                v-model="formData.password" 
                class="bg-zinc-900 mt-2 w-full p-2 rounded-lg" 
                placeholder="Enter your password" 
            />
            <input 
                type="password" 
                v-model="formData.confirmPassword" 
                class="bg-zinc-900 mt-2 w-full p-2 rounded-lg" 
                placeholder="Confirm your password" 
            />
        </form>

        <!-- Display all errors above the buttons -->
        <div v-if="errorMessages.length" class="mt-4 mb-2 p-2 bg-red-800 text-red-300 rounded-lg text-left">
            <ul>
                <li v-for="(message, index) in errorMessages" :key="index">
                    {{ message }}
                </li>
            </ul>
        </div>

        <div class="flex space-x-1">
            <router-link to="/" class="text-lg mt-3">
                <custom-button variant="secondary">
                    Go Back
                </custom-button>
            </router-link>
            <custom-button class="mt-3 w-full" @click="submitForm">
                Sign Up
            </custom-button>
        </div>
    </div>
</template>

<script setup>
import CustomButton from "../components/CustomButton.vue" 
import { makeHttpRequest } from "../composables/requests";
import { reactive, computed } from 'vue'

const formData = reactive({
    username: '',
    email: '',
    password: '',
    confirmPassword: ''
})

const errors = reactive({
    username: [],
    email: [],
    password: [],
    confirmPassword: []
})

// Compute all error messages as a single array
const errorMessages = computed(() => {
    return Object.values(errors).flat()
})

const submitForm = async () => {
    // Reset errors before making the request
    Object.keys(errors).forEach(field => {
        errors[field] = []
    })

    try {
        const response = await makeHttpRequest("POST", "http://localhost:5196/v1/api/auth/register", formData, {
            "Content-Type": "application/json",
        })

        console.log(response)
        
        if (!response.success) {
            // Handle validation errors
            Object.keys(response.data.errors).forEach(field => {
                errors[field] = response.data.errors[field]
            })
        } else {
            console.log('Registration successful', response.data)
            
            // TODO: register, redirect
        }
    } catch (error) {
        // Handle other errors
        console.error('An error occurred:', error)
    }
}
</script>
