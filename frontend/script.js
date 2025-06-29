// API Configuration
const API_BASE_URL = 'http://localhost:5016/api';

// Global state
let currentPage = 1;
let currentSearchPage = 1;
let currentPageSize = 10;
let currentSearchPageSize = 10;
let searchQuery = '';

// DOM Elements
const notification = document.getElementById('notification');

// Initialize the application
document.addEventListener('DOMContentLoaded', function() {
    loadFreelancers();
    setupEventListeners();
});

// Setup event listeners
function setupEventListeners() {
    // Create form submission
    document.getElementById('createForm').addEventListener('submit', handleCreateFreelancer);
    
    // Search input enter key
    document.getElementById('searchQuery').addEventListener('keypress', function(e) {
        if (e.key === 'Enter') {
            searchFreelancers();
        }
    });
}

// Tab functionality
function showTab(tabName) {
    // Hide all tab panes
    document.querySelectorAll('.tab-pane').forEach(pane => {
        pane.classList.remove('active');
    });
    
    // Remove active class from all tab buttons
    document.querySelectorAll('.tab-btn').forEach(btn => {
        btn.classList.remove('active');
    });
    
    // Show selected tab pane
    document.getElementById(tabName).classList.add('active');
    
    // Add active class to clicked button
    event.target.classList.add('active');
    
    // Load data for specific tabs
    if (tabName === 'list') {
        loadFreelancers();
    } else if (tabName === 'search') {
        // Reset search
        document.getElementById('searchResults').innerHTML = '<div class="no-results">Enter a search query to find freelancers</div>';
    }
}

// API Functions
async function apiCall(endpoint, options = {}) {
    const response = await fetch(API_BASE_URL + endpoint, options);
    if (!response.ok) {
        const errorText = await response.text();
        throw new Error(`HTTP ${response.status}: ${errorText}`);
    }
    // Only try to parse JSON if there is content
    const contentType = response.headers.get('content-type');
    if (contentType && contentType.includes('application/json')) {
        return await response.json();
    }
    return null; // For 204 No Content, etc.
}

// Load freelancers with pagination
async function loadFreelancers() {
    try {
        const pageSize = document.getElementById('pageSize').value;
        currentPageSize = parseInt(pageSize);
        const showArchived = document.getElementById('showArchivedToggle')?.checked;

        const response = await apiCall(`/freelancers?pageNumber=${currentPage}&pageSize=${currentPageSize}`);
        let freelancers = response.items;
        if (!showArchived) {
            freelancers = freelancers.filter(f => !f.isArchived);
        }
        displayFreelancers(freelancers, response.totalCount, response.pageNumber, response.totalPages);
        updatePaginationControls(response.hasPreviousPage, response.hasNextPage, response.pageNumber, response.totalPages);
    } catch (error) {
        showNotification('Error loading freelancers: ' + error.message, 'error');
    }
}

// Search freelancers
async function searchFreelancers() {
    const query = document.getElementById('searchQuery').value.trim();
    if (!query) {
        showNotification('Please enter a search query', 'warning');
        return;
    }
    
    try {
        searchQuery = query;
        const pageSize = document.getElementById('searchPageSize').value;
        currentSearchPageSize = parseInt(pageSize);
        
        const response = await apiCall(`/freelancers/search?query=${encodeURIComponent(query)}&pageNumber=${currentSearchPage}&pageSize=${currentSearchPageSize}`);
        
        displaySearchResults(response.items, response.totalCount, response.pageNumber, response.totalPages);
        updateSearchPaginationControls(response.hasPreviousPage, response.hasNextPage, response.pageNumber, response.totalPages);
        
    } catch (error) {
        showNotification('Error searching freelancers: ' + error.message, 'error');
    }
}

// Display freelancers in grid
function displayFreelancers(freelancers, totalCount, pageNumber, totalPages) {
    const container = document.getElementById('freelancersList');
    
    if (!freelancers || freelancers.length === 0) {
        container.innerHTML = '<div class="no-results">No freelancers found</div>';
        return;
    }
    
    const html = freelancers.map(freelancer => createFreelancerCard(freelancer)).join('');
    container.innerHTML = html;
}

// Display search results
function displaySearchResults(freelancers, totalCount, pageNumber, totalPages) {
    const container = document.getElementById('searchResults');
    
    if (!freelancers || freelancers.length === 0) {
        container.innerHTML = '<div class="no-results">No freelancers found matching your search</div>';
        return;
    }
    
    const html = freelancers.map(freelancer => createFreelancerCard(freelancer)).join('');
    container.innerHTML = html;
}

// Create freelancer card HTML
function createFreelancerCard(freelancer) {
    const skillsHtml = freelancer.skillsets.map(skill => `<span class="tag">${skill.name}</span>`).join('');
    const hobbiesHtml = freelancer.hobbies.map(hobby => `<span class="tag">${hobby.name}</span>`).join('');
    let archiveButton = '';
    if (freelancer.isArchived) {
        archiveButton = `<button class="btn btn-secondary" onclick="unarchiveFreelancer('${freelancer.id}')">Unarchive</button>`;
    } else {
        archiveButton = `<button class="btn btn-warning" onclick="archiveFreelancer('${freelancer.id}')">Archive</button>`;
    }
    return `
        <div class="freelancer-card">
            <div class="freelancer-header">
                <div class="freelancer-name">${freelancer.username}</div>
                ${freelancer.isArchived ? '<span class="archived-badge">Archived</span>' : ''}
            </div>
            <div class="freelancer-info">
                <div class="info-item">
                    <i class="fas fa-envelope"></i>
                    ${freelancer.email}
                </div>
                <div class="info-item">
                    <i class="fas fa-phone"></i>
                    ${freelancer.phoneNumber}
                </div>
            </div>
            <div class="skills-section">
                <div class="section-title">Skills</div>
                <div class="tags">${skillsHtml || '<span class="tag">No skills listed</span>'}</div>
            </div>
            <div class="hobbies-section">
                <div class="section-title">Hobbies</div>
                <div class="tags">${hobbiesHtml || '<span class="tag">No hobbies listed</span>'}</div>
            </div>
            <div class="card-actions">${archiveButton}</div>
        </div>
    `;
}

// Pagination controls
function updatePaginationControls(hasPrevious, hasNext, pageNumber, totalPages) {
    document.getElementById('prevBtn').disabled = !hasPrevious;
    document.getElementById('nextBtn').disabled = !hasNext;
    document.getElementById('pageInfo').textContent = `Page ${pageNumber} of ${totalPages}`;
}

function updateSearchPaginationControls(hasPrevious, hasNext, pageNumber, totalPages) {
    document.getElementById('searchPrevBtn').disabled = !hasPrevious;
    document.getElementById('searchNextBtn').disabled = !hasNext;
    document.getElementById('searchPageInfo').textContent = `Page ${pageNumber} of ${totalPages}`;
}

function previousPage() {
    if (currentPage > 1) {
        currentPage--;
        loadFreelancers();
    }
}

function nextPage() {
    currentPage++;
    loadFreelancers();
}

function previousSearchPage() {
    if (currentSearchPage > 1) {
        currentSearchPage--;
        searchFreelancers();
    }
}

function nextSearchPage() {
    currentSearchPage++;
    searchFreelancers();
}

// Create freelancer form handling
async function handleCreateFreelancer(event) {
    event.preventDefault();
    
    const formData = new FormData(event.target);
    const freelancerData = {
        username: formData.get('username'),
        email: formData.get('email'),
        phoneNumber: formData.get('phoneNumber'),
        isArchived: false,
        skillsets: getSkillsets(),
        hobbies: getHobbies()
    };
    
    try {
        await apiCall('/freelancers', {
            method: 'POST',
            body: JSON.stringify(freelancerData)
        });
        
        showNotification('Freelancer created successfully!', 'success');
        event.target.reset();
        resetSkillsAndHobbies();
        
        // Switch to list tab and refresh
        showTab('list');
        loadFreelancers();
        
    } catch (error) {
        showNotification('Error creating freelancer: ' + error.message, 'error');
    }
}

// Skills and hobbies management
function addSkill() {
    const container = document.getElementById('skillsetsContainer');
    const skillItem = document.createElement('div');
    skillItem.className = 'skill-item';
    skillItem.innerHTML = `
        <input type="text" class="skill-input" placeholder="Enter skill name">
        <button type="button" class="remove-btn" onclick="removeSkill(this)">
            <i class="fas fa-times"></i>
        </button>
    `;
    container.appendChild(skillItem);
}

function removeSkill(button) {
    button.parentElement.remove();
}

function addHobby() {
    const container = document.getElementById('hobbiesContainer');
    const hobbyItem = document.createElement('div');
    hobbyItem.className = 'hobby-item';
    hobbyItem.innerHTML = `
        <input type="text" class="hobby-input" placeholder="Enter hobby name">
        <button type="button" class="remove-btn" onclick="removeHobby(this)">
            <i class="fas fa-times"></i>
        </button>
    `;
    container.appendChild(hobbyItem);
}

function removeHobby(button) {
    button.parentElement.remove();
}

function getSkillsets() {
    const skills = [];
    document.querySelectorAll('.skill-input').forEach(input => {
        if (input.value.trim()) {
            skills.push({ name: input.value.trim() });
        }
    });
    return skills;
}

function getHobbies() {
    const hobbies = [];
    document.querySelectorAll('.hobby-input').forEach(input => {
        if (input.value.trim()) {
            hobbies.push({ name: input.value.trim() });
        }
    });
    return hobbies;
}

function resetSkillsAndHobbies() {
    document.getElementById('skillsetsContainer').innerHTML = `
        <div class="skill-item">
            <input type="text" class="skill-input" placeholder="Enter skill name">
            <button type="button" class="remove-btn" onclick="removeSkill(this)">
                <i class="fas fa-times"></i>
            </button>
        </div>
    `;
    
    document.getElementById('hobbiesContainer').innerHTML = `
        <div class="hobby-item">
            <input type="text" class="hobby-input" placeholder="Enter hobby name">
            <button type="button" class="remove-btn" onclick="removeHobby(this)">
                <i class="fas fa-times"></i>
            </button>
        </div>
    `;
}

// Validation testing functions
async function testInvalidUsername() {
    const testData = {
        username: "ab",
        email: "test@example.com",
        phoneNumber: "+1234567890",
        skillsets: [],
        hobbies: []
    };
    
    try {
        await apiCall('/freelancers', {
            method: 'POST',
            body: JSON.stringify(testData)
        });
    } catch (error) {
        displayValidationResult('Invalid Username Test', error.message);
    }
}

async function testInvalidEmail() {
    const testData = {
        username: "testuser",
        email: "invalid-email",
        phoneNumber: "+1234567890",
        skillsets: [],
        hobbies: []
    };
    
    try {
        await apiCall('/freelancers', {
            method: 'POST',
            body: JSON.stringify(testData)
        });
    } catch (error) {
        displayValidationResult('Invalid Email Test', error.message);
    }
}

async function testInvalidPhone() {
    const testData = {
        username: "testuser",
        email: "test@example.com",
        phoneNumber: "123",
        skillsets: [],
        hobbies: []
    };
    
    try {
        await apiCall('/freelancers', {
            method: 'POST',
            body: JSON.stringify(testData)
        });
    } catch (error) {
        displayValidationResult('Invalid Phone Test', error.message);
    }
}

async function testInvalidPagination() {
    try {
        await apiCall('/freelancers?pageNumber=0&pageSize=100');
    } catch (error) {
        displayValidationResult('Invalid Pagination Test', error.message);
    }
}

function displayValidationResult(testName, result) {
    const output = document.getElementById('validationOutput');
    const timestamp = new Date().toLocaleTimeString();
    
    output.textContent = `[${timestamp}] ${testName}:\n${result}\n\n${output.textContent}`;
}

// Notification system
function showNotification(message, type = 'info') {
    notification.textContent = message;
    notification.className = `notification ${type} show`;
    
    setTimeout(() => {
        notification.classList.remove('show');
    }, 5000);
}

// Utility functions
function formatDate(dateString) {
    return new Date(dateString).toLocaleDateString();
}

// Error handling
window.addEventListener('error', function(e) {
    console.error('Global error:', e.error);
    showNotification('An unexpected error occurred', 'error');
});

// Handle API connection errors
window.addEventListener('unhandledrejection', function(e) {
    console.error('Unhandled promise rejection:', e.reason);
    showNotification('Network error: Please check if the API is running', 'error');
});

async function archiveFreelancer(id) {
    try {
        await apiCall(`/freelancers/${id}/archive`, { method: 'PATCH' });
        showNotification('Freelancer archived.', 'success');
        loadFreelancers();
    } catch (error) {
        showNotification('Error archiving freelancer: ' + error.message, 'error');
    }
}

async function unarchiveFreelancer(id) {
    try {
        await apiCall(`/freelancers/${id}/unarchive`, { method: 'PATCH' });
        showNotification('Freelancer unarchived.', 'success');
        loadFreelancers();
    } catch (error) {
        showNotification('Error unarchiving freelancer: ' + error.message, 'error');
    }
} 