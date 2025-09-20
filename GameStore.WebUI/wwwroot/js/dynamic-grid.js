// Dynamic Grid Height Manager
class DynamicGridManager {
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
            availableHeight = Math.max(250, availableHeight);
        } else if (viewportHeight <= 768) {
            availableHeight = Math.max(300, availableHeight);
        } else if (viewportHeight >= 900) {
            availableHeight = Math.min(900, availableHeight);
        } else {
            availableHeight = Math.max(400, Math.min(800, availableHeight));
        }

        // Apply the calculated height
        containerElement.style.height = `${availableHeight}px`;
        
        // Force Radzen DataGrid to refresh its layout
        const dataGrid = containerElement.querySelector('.rz-datagrid');
        if (dataGrid) {
            // Trigger a resize event on the DataGrid
            setTimeout(() => {
                if (dataGrid._resize) {
                    dataGrid._resize();
                }
                // Alternative method for Radzen
                const event = new Event('resize');
                dataGrid.dispatchEvent(event);
            }, 50);
        }
    }

    // Public method to manually refresh all grids
    refreshAll() {
        this.updateAllGrids();
    }
}

// Global instance
window.dynamicGridManager = new DynamicGridManager();

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

window.refreshAllDynamicGrids = () => {
    if (window.dynamicGridManager) {
        window.dynamicGridManager.refreshAll();
    }
};

// Export for module systems
if (typeof module !== 'undefined' && module.exports) {
    module.exports = DynamicGridManager;
}
