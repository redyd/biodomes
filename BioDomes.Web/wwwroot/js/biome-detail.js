const btn = document.getElementById("cy-recenter");

const removeSuggestionsBtns = document.querySelectorAll(".btn-suggestion-del");

removeSuggestionsBtns.forEach(btn => {
    btn.addEventListener("click", (e) => {
        const listItem = btn.closest("li");

        if (listItem) {
            listItem.style.opacity = '0';
            listItem.style.transform = 'translateX(20px)';
            listItem.style.transition = 'all 0.3s ease';

            setTimeout(() => {
                listItem.remove();
            }, 300);
        }
    });
});

(function () {
    const data = window.foodChainData;

    if (!data) {
        console.error("foodChainData is missing");
        return;
    }
    
    console.log(data);

    const nodes = data.Nodes || [];
    const edges = data.Edges || [];

    const elements = [
        ...nodes.map(n => ({
            data: {
                id: n.SpecieId,
                label: `${n.Name}\n${n.Count}`,
                count: n.Count
            }
        })),
        ...edges.map(e => ({
            data: {
                source: e.PredatorId,
                target: e.PreyId
            }
        }))
    ];

    const cy = cytoscape({
        container: document.getElementById('cy'),
        maxZoom: 1.5,
        minZoom: 0.2,
        elements,

        style: [
            {
                selector: 'node',
                style: {
                    'width': function(ele) {
                        const count = ele.data('count') || 0;
                        return Math.min(60 + (count * 4), 140) + 'px';
                    },
                    'height': function(ele) {
                        const count = ele.data('count') || 0;
                        return Math.min(60 + (count * 4), 140) + 'px';
                    },
                    'font-size': function(ele) {
                        const count = ele.data('count') || 0;
                        return Math.min(12 + (count * 0.5), 18) + 'px';
                    },

                    'background-color': '#132830',
                    'label': 'data(label)',
                    'color': '#e8f4f2',
                    'font-family': 'var(--sans)',
                    'font-weight': '600',
                    'text-valign': 'center',
                    'text-halign': 'center',
                    'text-wrap': 'wrap',
                    'text-max-width': 80,
                    'line-height': 1.2,

                    'border-width': 1.5,
                    'border-color': 'rgba(0, 210, 180, 0.4)',

                    'text-outline-width': 2,
                    'text-outline-color': '#060d0f',

                    'transition-property': 'background-color, border-color, border-width',
                    'transition-duration': '0.2s'
                }
            },

            {
                selector: 'edge',
                style: {
                    'width': 2,
                    'line-color': 'rgba(122, 173, 168, 0.3)',
                    'target-arrow-color': 'rgba(0, 210, 180, 0.4)',
                    'target-arrow-shape': 'triangle',
                    'curve-style': 'bezier',
                    'arrow-scale': 1.2,
                    'opacity': 0.8,
                    'ghost': 'yes',
                    'ghost-offset-x': 0,
                    'ghost-offset-y': 1,
                    'ghost-opacity': 0.1
                }
            },

            {
                selector: ':selected',
                style: {
                    'border-color': '#ffffff',
                    'background-color': '#1a3a42'
                }
            },

            {
                selector: 'node[count = 0]',
                style: {
                    'background-color': '#2c2c2c',
                    'border-color': '#555555',
                    'opacity': 0.6,
                    'color': '#888888'
                }
            }
        ],

        layout: {
            name: 'breadthfirst',
            directed: true,
            padding: 40,
            spacingFactor: 1.7,
            animate: false,
        }
    });

    btn.addEventListener("click", (e) => {
        cy.fit(null, 50)
    })

    setTimeout(() => {
        cy.resize();
        cy.fit();
    }, 50);

    window.addEventListener('resize', () => {
        cy.resize();
    });

})();
