// Dynamic Grid Height Manager
// Check if already exists to prevent redeclaration
if (typeof window.DynamicGridManager === 'undefined') {
    window.DynamicGridManager = class {
        constructor() {
            this.grids = new Set();
            this.resizeObserver = null;
            this.init();
        }

    init() {
        // Wait for DOM to be ready
        if (document.readyState === 'loading') {
            document.addEventListener('DOMContentLoaded', () => this.setupResizeObserver());
        } else {
            this.setupResizeObserver();
        }

        // Listen for window resize events
        window.addEventListener('resize', () => this.handleResize());
        
        // Listen for orientation change on mobile
        window.addEventListener('orientationchange', () => {
            setTimeout(() => this.handleResize(), 100);
        });
    }

    setupResizeObserver() {
        // Use ResizeObserver for more precise resize detection
        if (window.ResizeObserver) {
            this.resizeObserver = new ResizeObserver(entries => {
                this.handleResize();
            });
            
            // Observe the main content area
            const mainContent = document.querySelector('main');
            if (mainContent) {
                this.resizeObserver.observe(mainContent);
            }
        }
    }

    registerGrid(containerElement) {
        if (containerElement) {
            this.grids.add(containerElement);
            this.updateGridHeight(containerElement);
        }
    }

    unregisterGrid(containerElement) {
        this.grids.delete(containerElement);
    }

    handleResize() {
        // Debounce resize events
        clearTimeout(this.resizeTimeout);
        this.resizeTimeout = setTimeout(() => {
            this.updateAllGrids();
        }, 100);
    }

    updateAllGrids() {
        this.grids.forEach(grid => {
            this.updateGridHeight(grid);
        });
    }

    updateGridHeight(containerElement) {
        if (!containerElement) return;

        const viewportHeight = window.innerHeight;
        const viewportWidth = window.innerWidth;
        
        // Calculate available height
        let availableHeight = viewportHeight;
        
        // Subtract header height (approximately)
        availableHeight -= 120; // Header + navigation
        
        // Subtract footer/padding (approximately)
        availableHeight -= 40;
        
        // Adjust for different screen sizes
        if (viewportHeight <= 600) {
            availableHeight = Math.max(300, availableHeight);
        } else if (viewportHeight <= 768) {
            availableHeight = Math.max(350, availableHeight);
        } else if (viewportHeight >= 900) {
            availableHeight = Math.min(700, availableHeight); // Reduced max height to ensure pagination is visible
        } else {
            availableHeight = Math.max(450, Math.min(650, availableHeight)); // Reduced max height
        }

        // Apply the calculated height with max-height to ensure pagination is visible
        containerElement.style.height = `${availableHeight}px`;
        containerElement.style.maxHeight = `${availableHeight}px`;
        
        // Force Radzen DataGrid to refresh its layout
        const dataGrid = containerElement.querySelector('.rz-datagrid');
        if (dataGrid) {
            // Set the DataGrid to use flex layout for better pagination visibility
            dataGrid.style.height = 'auto';
            dataGrid.style.maxHeight = `${availableHeight - 100}px`; // Leave space for pagination
            
            // Trigger a resize event on the DataGrid
            setTimeout(() => {
                if (dataGrid._resize) {
                    dataGrid._resize();
                }
                // Alternative method for Radzen
                const event = new Event('resize');
                dataGrid.dispatchEvent(event);
                
                // Force pagination to be visible
                const pagination = dataGrid.querySelector('.rz-paginator');
                if (pagination) {
                    pagination.style.display = 'block';
                    pagination.style.visibility = 'visible';
                }
            }, 50);
        }
    }

        // Public method to manually refresh all grids
        refreshAll() {
            this.updateAllGrids();
        }
    };
}

// Global instance - only create if it doesn't exist
if (!window.dynamicGridManager) {
    window.dynamicGridManager = new window.DynamicGridManager();
}

// Blazor interop functions
window.registerDynamicGrid = (element) => {
    if (window.dynamicGridManager) {
        window.dynamicGridManager.registerGrid(element);
    }
};

window.refreshDynamicGrid = (element) => {
    if (window.dynamicGridManager) {
        window.dynamicGridManager.updateGridHeight(element);
    }
};

window.unregisterDynamicGrid = (element) => {
    if (window.dynamicGridManager) {
        window.dynamicGridManager.unregisterGrid(element);
    }
};

window.refreshAllDynamicGrids = () => {
    if (window.dynamicGridManager) {
        window.dynamicGridManager.refreshAll();
    }
};

window.ensurePaginationVisible = (element) => {
    if (element) {
        const dataGrid = element.querySelector('.rz-datagrid');
        if (dataGrid) {
            const pagination = dataGrid.querySelector('.rz-paginator');
            if (pagination) {
                pagination.style.display = 'block';
                pagination.style.visibility = 'visible';
                pagination.style.opacity = '1';
                pagination.style.position = 'relative';
                pagination.style.zIndex = '100';
                pagination.style.background = 'white';
                pagination.style.borderTop = '1px solid #e9ecef';
                pagination.style.padding = '0.5rem';
            }
        }
    }
};

// Global function to ensure all paginations are visible
window.ensureAllPaginationsVisible = () => {
    const allDataGrids = document.querySelectorAll('.rz-datagrid');
    allDataGrids.forEach(dataGrid => {
        const pagination = dataGrid.querySelector('.rz-paginator');
        if (pagination) {
            pagination.style.display = 'block';
            pagination.style.visibility = 'visible';
            pagination.style.opacity = '1';
            pagination.style.position = 'relative';
            pagination.style.zIndex = '100';
            pagination.style.background = 'white';
            pagination.style.borderTop = '1px solid #e9ecef';
            pagination.style.padding = '0.5rem';
        }
    });
};

// Auto-execute on page load
document.addEventListener('DOMContentLoaded', () => {
    setTimeout(() => {
        window.ensureAllPaginationsVisible();
    }, 1000);
});

// Export for module systems
if (typeof module !== 'undefined' && module.exports) {
    module.exports = window.DynamicGridManager;
}
